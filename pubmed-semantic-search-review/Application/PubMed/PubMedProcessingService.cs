using DhsResearchLibrary.Infrastructure.OpenAI;
using Microsoft.Extensions.Options;
using PubMedSemanticSearchReview.Application.Data;
using PubMedSemanticSearchReview.Application.Interfaces;
using PubMedSemanticSearchReview.Application.OpenAi;
using PubMedSemanticSearchReview.Infrastructure;
using PubMedSemanticSearchReview.Infrastructure.Configuration;
using Serilog;

namespace PubMedSemanticSearchReview.Application.PubMed
{
    public class PubMedProcessingService(
        IPubMedService pubMedService,
        IPubmedArticleSetService pubMedArticleSetService,
        IChatCompletionService chatCompletionService,
        ICsvService<ArticleDto> csvService,
        ILogger logger,
        IOptions<OpenAiConfig> openAiConfigOptions, IOptions<PubMedConfig> pubMedConfigOptions) : IPubMedProcessingService
    {
        private readonly IPubMedService _pubMedService = pubMedService;
        private readonly IPubmedArticleSetService _pubMedArticleSetService = pubMedArticleSetService;
        private readonly IChatCompletionService _chatCompletionService = chatCompletionService;
        private readonly ICsvService<ArticleDto> _csvService = csvService;
        private readonly ILogger _logger = logger;
        private readonly OpenAiConfig _openAiConfig = openAiConfigOptions.Value;
        private readonly PubMedConfig _pubMedConfig = pubMedConfigOptions.Value;

        public async Task ProcessPubMedSearchTermsAsync(string articleOutputSavePath)
        {
            var pubMedSearchTerms = PubMedStartupService.PrepareSearchQuery(_logger);
            var articleResults = new Dictionary<string, List<ArticleDto>>();

            foreach (var pubMedSearchTerm in pubMedSearchTerms)
            {
                _logger.Information("Processing PubMed search term: {PubMedSearchTerm}", pubMedSearchTerm);
                var articles = await ProcessSearchTermAsync(pubMedSearchTerm.SearchTerm, pubMedSearchTerm.StartDate, pubMedSearchTerm.EndDate);
                articleResults.Add(pubMedSearchTerm.SearchTerm, articles);
            }

            SaveResults(articleOutputSavePath, articleResults);
        }

        private async Task<List<ArticleDto>> ProcessSearchTermAsync(string pubMedSearchTerm, DateTime? startDate = null, DateTime? endDate = null)
        {
            var articles = new List<ArticleDto>();
            var pmIds = await _pubMedService.BasicSearchAsync(pubMedSearchTerm, startDate, endDate);

            if(pmIds.Count > 100)
            {
                _logger.Warning("{count} articles found for search term: {PubMedSearchTerm}", pmIds.Count, pubMedSearchTerm);

                Console.WriteLine($"{pmIds.Count} articles found for search term: {pubMedSearchTerm}. Do you wish to proceed? Press 'Enter' to continue.");
                var key = Console.ReadKey().Key;
                if (key != ConsoleKey.Enter)
                {
                    _logger.Information("Exiting...");
                    return articles;
                }
            }
            else
            {
                _logger.Information("Found {PubMedArticleCount} articles for search term: {PubMedSearchTerm}", pmIds.Count, pubMedSearchTerm);
            }
         
            int totalCount = pmIds.Count;
            foreach (var pmId in pmIds)
            {
                _logger.Information("Processing PubMed article: {PubMedArticleId}. Remaining: {Remaining}", pmId, --totalCount);
                var article = await ProcessArticleAsync(pmId, pubMedSearchTerm);
                if (article != null)
                {
                    articles.Add(article);
                }
            }

            return articles;
        }

        private async Task<ArticleDto?> ProcessArticleAsync(long pmId, string pubMedSearchTerm)
        {
            var pubmedArticleXml = await _pubMedService.GetFullXmlAsync(pmId);
            var pubMedArticle = _pubMedArticleSetService.GetPubMedArticleFromXml(pubmedArticleXml, pubMedSearchTerm);

            if (pubMedArticle == null)
            {
                _logger.Warning("Article not found for PubMed article: {PubMedArticleId}", pmId);
                return null;
            }

            var article = ArticleDto.Create(pubMedSearchTerm, pubMedArticle, _pubMedConfig.BaseArticleUrl);

            if (string.IsNullOrEmpty(article.ArticleTitle) || string.IsNullOrEmpty(article.AbstractText))
            {
                _logger.Warning("Article title or abstract not found for PubMed article: {PubMedArticleId}", pmId);
                return null;
            }

            var promptForAbstractAnalysis = PromptService.GetPromptForAbstractAnalysis(article.ArticleTitle, article.AbstractText);
            var structuredOutput = await _chatCompletionService.GetStructuredAbstractAnalysisResponseAsync(
                promptForAbstractAnalysis.SystemPrompt,
                promptForAbstractAnalysis.UserPrompt);

            if (structuredOutput == null)
            {
                _logger.Warning("Structured output not found for PubMed article: {PubMedArticleId}", pmId);
                return null;
            }

            article.UpdateWithStructuredOutput(structuredOutput, _openAiConfig.TokenPricing);
            return article;
        }

        private void SaveResults(string articleOutputSavePath, Dictionary<string, List<ArticleDto>> articleResults)
        {
            foreach (var (searchTerm, articles) in articleResults)
            {
                var sanitizedTerm = SanitizeFileName(searchTerm);
                var savePath = Path.Combine(articleOutputSavePath, sanitizedTerm);

                if (!Directory.Exists(savePath))
                {
                    _logger.Information("Creating directory: {SavePath}", savePath);
                    Directory.CreateDirectory(savePath);
                }

                var csvPath = Path.Combine(savePath, "articles.csv");
                _logger.Information("Saving articles to: {CsvPath}", csvPath);
                _csvService.WriteCsv(csvPath, articles);

                OpenFolder(savePath);
            }
        }

        private static string SanitizeFileName(string fileName) => string.Concat(fileName.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));

        private static void OpenFolder(string folderPath)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = folderPath,
                UseShellExecute = true,
                Verb = "open"
            });
        }
    }
}
