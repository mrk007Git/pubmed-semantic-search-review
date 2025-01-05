using DhsResearchLibrary.Infrastructure.OpenAI;
using PubMedSemanticSearchReview.Application.Data;
using PubMedSemanticSearchReview.Application.Interfaces;
using PubMedSemanticSearchReview.Application.OpenAi;
using PubMedSemanticSearchReview.Infrastructure;
using Serilog;

namespace PubMedSemanticSearchReview.Application.PubMed
{
    public class PubMedProcessingService(
        IPubMedService pubMedService,
        IPubmedArticleSetService pubMedArticleSetService,
        IChatCompletionService chatCompletionService,
        ICsvService<ArticleDto> csvService,
        ILogger logger) : IPubMedProcessingService
    {
        private readonly IPubMedService _pubMedService = pubMedService;
        private readonly IPubmedArticleSetService _pubMedArticleSetService = pubMedArticleSetService;
        private readonly IChatCompletionService _chatCompletionService = chatCompletionService;
        private readonly ICsvService<ArticleDto> _csvService = csvService;
        private readonly ILogger _logger = logger;

        public async Task ProcessPubMedSearchTermsAsync( string articleOutputSavePath)
        {
            var pubMedSearchTerms = PubMedStartupService.PrepareSearchQuery(_logger);

            List<ArticleDto> articles;
            var articleResults = new Dictionary<string, List<ArticleDto>>();

            foreach (var pubMedSearchTerm in pubMedSearchTerms)
            {
                _logger.Information("Processing PubMed search term: {PubMedSearchTerm}", pubMedSearchTerm);
                articles = new List<ArticleDto>(); // Reset articles for each search term
                var pmIds = await _pubMedService.BasicSearchAsync(pubMedSearchTerm);
                var idCount = pmIds.Count;

                foreach (var pmId in pmIds)
                {
                    _logger.Information("Processing PubMed article: {PubMedArticleId}. Articles remaining to process: {count}", pmId, idCount--);
                    var pubmedArticleXml = await _pubMedService.GetFullXmlAsync(pmId);
                    var pubMedArticle = _pubMedArticleSetService.GetPubMedArticleFromXml(pubmedArticleXml); // Converts the PubMed XML to an ArticleDto

                    if (pubMedArticle == null)
                    {
                        _logger.Warning("Article not found for PubMed article: {PubMedArticleId}", pmId);
                        continue;
                    }

                    var article = ArticleDto.Create(pubMedSearchTerm, pubMedArticle);

                    if (string.IsNullOrEmpty(article.ArticleTitle) || string.IsNullOrEmpty(article.AbstractText))
                    {
                        _logger.Warning("Article title or abstract not found for PubMed article: {PubMedArticleId}", pmId);
                        continue;
                    }

                    var promptForAbstractAnalysis = PromptService.GetPromptForAbstractAnalysis(article.ArticleTitle, article.AbstractText);

                    var articleStructuredOutput = await _chatCompletionService.GetStructuredAbstractAnalysisResponseAsync(promptForAbstractAnalysis.SystemPrompt, promptForAbstractAnalysis.UserPrompt);

                    if (articleStructuredOutput == null)
                    {
                        _logger.Warning("Structured output not found for PubMed article: {PubMedArticleId}", pmId);
                        continue;
                    }

                    article.UpdateWithStructuredOutput(articleStructuredOutput);

                    articles.Add(article);
                    break; // TODO Remove before flight
                }
                articleResults.Add(pubMedSearchTerm, articles);
            }

            foreach (var articleResult in articleResults)
            {
                var sanitizedTerm = string.Concat(articleResult.Key.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
                var savePath = Path.Combine(articleOutputSavePath, sanitizedTerm);
                if (!Directory.Exists(savePath))
                {
                    _logger.Information("Creating directory: {SavePath}", savePath);
                    Directory.CreateDirectory(savePath);
                }

                var csvPath = Path.Combine(savePath, "articles.csv");
                _logger.Information("Saving articles to: {CsvPath}", csvPath);
                // Write the articles to a CSV file
                _csvService.WriteCsv(csvPath, articleResult.Value);
                // Open the folder
                System.Diagnostics.Process.Start("explorer.exe", savePath);
            }
        }
    }
}
