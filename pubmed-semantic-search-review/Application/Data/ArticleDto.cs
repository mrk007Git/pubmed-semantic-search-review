namespace PubMedSemanticSearchReview.Application.Data;

public partial class ArticleDto
{
    public long PmId { get; set; }
    public DateTime? DateComplete { get; set; }
    public DateTime? DateRevised { get; set; }
    public string? JournalName { get; set; }
    public string? ArticleTitle { get; set; }
    public string? AbstractText { get; set; }

    public string? ArticleUrl { get; set; }

    public bool? IsRelevant { get; set; }
    public decimal? EstimatedPercentRelevant { get; set; }
    public string? AbstractSummary { get; set; }
    public string? RelevanceReason { get; set; }
    public DateTime? DateProcessed { get; set; }

    public int? PromptTokens { get; set; }
    public int? CompletionTokens { get; set; }

    public double? EstimatedTotalCost { get; set; }

    public string? SearchTerm { get; set; }

    public static ArticleDto Create(string searchTerm, PubMed.PubMedArticleDto pubMedArticleDto, string articleBaseUrl)
    {
        return new ArticleDto
        {
            SearchTerm = searchTerm,
            AbstractText = pubMedArticleDto.AbstractText,
            ArticleTitle = pubMedArticleDto.ArticleTitle,
            AbstractSummary = null,
            DateComplete = pubMedArticleDto.DateCompleted,
            DateProcessed = null,
            DateRevised = pubMedArticleDto.DateRevised,
            IsRelevant = null,
            EstimatedPercentRelevant = null,
            RelevanceReason = null,
            JournalName = pubMedArticleDto.JournalName,
            PmId = pubMedArticleDto.PmId,
            PromptTokens = null,
            CompletionTokens = null,
            EstimatedTotalCost = null,
            ArticleUrl = articleBaseUrl.EndsWith("/") ? $"{articleBaseUrl}{pubMedArticleDto.PmId}" : $"{articleBaseUrl}/{pubMedArticleDto.PmId}"
        };
    }
}
