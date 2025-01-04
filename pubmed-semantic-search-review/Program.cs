using DhsResearchLibrary.Infrastructure.OpenAI;
using Microsoft.Extensions.DependencyInjection;
using PubMedSemanticSearchReview.Application;
using PubMedSemanticSearchReview.Application.Data;
using PubMedSemanticSearchReview.Application.Interfaces;
using PubMedSemanticSearchReview.Application.OpenAi;
using PubMedSemanticSearchReview.Application.PubMed;
using PubMedSemanticSearchReview.Infrastructure;
using Serilog;
using PubMedConfigurationBuilder = PubMedSemanticSearchReview.Infrastructure.ConfigurationBuilder;

Console.WriteLine("Welcome to the PubMed Semantic Search Review!");

var configuration = PubMedConfigurationBuilder.GetConfiguration(new Microsoft.Extensions.Configuration.ConfigurationBuilder());

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

string[] pubMedSearchTerms = File.ReadAllLines(@"Data\PubMedSearchTerms.txt");

if(pubMedSearchTerms.Length == 0)
{
    logger.Error("No PubMed search terms found in Data\\PubMedSearchTerms.txt. Exiting...");
    return;
}

Console.WriteLine($"Are you sure you want to process {string.Join(", ", pubMedSearchTerms)}? (Press ENTER to continue)");

if (Console.ReadKey().Key != ConsoleKey.Enter)
{
    Console.WriteLine("Exiting...");
    Console.WriteLine();
    return;
}

var duplicateTerms = pubMedSearchTerms
    .GroupBy(term => term)
    .Where(group => group.Count() > 1)
    .Select(group => group.Key)
    .ToList();

if (duplicateTerms.Any())
{
    logger.Warning("Duplicate search terms found: {DuplicateTerms}. Duplicates will be removed!", string.Join(", ", duplicateTerms));
}

pubMedSearchTerms = pubMedSearchTerms.Distinct().ToArray();

var articleOutputSavePath = configuration["ArticleOutputSavePath"];

if (string.IsNullOrEmpty(articleOutputSavePath))
{
    logger.Error("ArticleOutputSavePath not found in configuration. Exiting...");
    return;
}

var services = new ServiceCollection();
services.AddInfrastructure(configuration, logger);
services.AddApplication(logger);

var serviceProvider = services.BuildServiceProvider();

var pubMedService = serviceProvider.GetRequiredService<IPubMedService>();
var pubMedArticleSetService = serviceProvider.GetRequiredService<IPubmedArticleSetService>();
var chatCompletionService = serviceProvider.GetRequiredService<IChatCompletionService>();

List<ArticleDto> articles;
var articleResults = new Dictionary<string, List<ArticleDto>>();

foreach (var pubMedSearchTerm in pubMedSearchTerms)
{
    logger.Information("Processing PubMed search term: {PubMedSearchTerm}", pubMedSearchTerm);
    articles = []; //Reset articles for each search term
    var pmIds = await pubMedService.BasicSearchAsync(pubMedSearchTerm);
    var idCount = pmIds.Count;

    foreach (var pmId in pmIds)
    {
        logger.Information("Processing PubMed article: {PubMedArticleId}. Articles remaining to process: {count}", pmId, idCount--);
        var pubmedArticleXml = await pubMedService.GetFullXmlAsync(pmId);
        var pubMedArticle =  pubMedArticleSetService.GetPubMedArticleFromXml(pubmedArticleXml); //Converts the PubMed XML to an ArticleDto

        if(pubMedArticle == null)
        {
            logger.Warning("Article not found for PubMed article: {PubMedArticleId}", pmId);
            continue;
        }

        var article = ArticleDto.Create(pubMedSearchTerm, pubMedArticle);

        if(string.IsNullOrEmpty(article.ArticleTitle) || string.IsNullOrEmpty(article.AbstractText))
        {
            logger.Warning("Article title or abstract not found for PubMed article: {PubMedArticleId}", pmId);
            continue;
        }

        var promptForAbstractAnalysis = PromptService.GetPromptForAbstractAnalysis(article.ArticleTitle, article.AbstractText);

        var articleStructuredOutput = await chatCompletionService.GetStructuredAbstractAnalysisResponseAsync(promptForAbstractAnalysis.SystemPrompt, promptForAbstractAnalysis.UserPrompt);

        if(articleStructuredOutput == null)
        {
            logger.Warning("Structured output not found for PubMed article: {PubMedArticleId}", pmId);
            continue;
        }

        article.UpdateWithStructuredOutput(articleStructuredOutput);

        articles.Add(article);
        break; //TODO Remove before flight
    }
    articleResults.Add(pubMedSearchTerm, articles);
}

foreach(var articleResult in articleResults)
{
    var sanitizedTerm = string.Concat(articleResult.Key.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
    var savePath = Path.Combine(articleOutputSavePath, sanitizedTerm);
    if (!Directory.Exists(savePath))
    {
        logger.Information("Creating directory: {SavePath}", savePath);
        Directory.CreateDirectory(savePath);
    }

    var csvPath = Path.Combine(savePath, "articles.csv");
    logger.Information("Saving articles to: {CsvPath}", csvPath);
    //Write the articles to a CSV file
    serviceProvider.GetRequiredService<ICsvService<ArticleDto>>().WriteCsv(csvPath, articleResult.Value);
    //Open the folder
    System.Diagnostics.Process.Start("explorer.exe", savePath);
}
