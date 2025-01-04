using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using PubMedSemanticSearchReview.Application;
using PubMedSemanticSearchReview.Application.Interfaces;
using PubMedSemanticSearchReview.Infrastructure;
using Serilog;
using PubMedConfigurationBuilder = PubMedSemanticSearchReview.Infrastructure.ConfigurationBuilder;

Console.WriteLine("Welcome to the PubMed Semantic Search Review!");

string[] pubMedSearchTerms = File.ReadAllLines(@"Data\PubMedSearchTerms.txt");

Console.WriteLine($"Are you sure you want to process {string.Join(", ", pubMedSearchTerms)}? (Press ENTER to continue)");

if (Console.ReadKey().Key != ConsoleKey.Enter)
{
    Console.WriteLine("Exiting...");
    Console.WriteLine();
    return;
}

var configuration = PubMedConfigurationBuilder.GetConfiguration(new Microsoft.Extensions.Configuration.ConfigurationBuilder());

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var services = new ServiceCollection();
services.AddInfrastructure(configuration, logger);
services.AddApplication(logger);

var serviceProvider = services.BuildServiceProvider();

var pubMedService = serviceProvider.GetRequiredService<IPubMedService>();

foreach (var pubMedSearchTerm in pubMedSearchTerms)
{
    logger.Information("Processing PubMed search term: {PubMedSearchTerm}", pubMedSearchTerm);
    var pmIds = await pubMedService.BasicSearchAsync(pubMedSearchTerm);
    var idCount = pmIds.Count;

    foreach (var pmId in pmIds)
    {
        logger.Information("Processing PubMed article: {PubMedArticleId}. Articles remaining to process: {count}", id, idCount--);
        var article = await pubMedService.GetFullXmlAsync(pmId);
        Console.WriteLine(article);
    }
}
