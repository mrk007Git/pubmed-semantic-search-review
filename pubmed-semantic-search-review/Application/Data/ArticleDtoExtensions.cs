using PubMedSemanticSearchReview.Application.OpenAi.AbstractAnalysis;

namespace PubMedSemanticSearchReview.Application.Data;
public static class ArticleDtoExtensions
{
    public static void UpdateWithStructuredOutput(this ArticleDto article, StructuredResponseDto structuredResponseDto)
    {
        article.AbstractSummary = structuredResponseDto.AbstractSummary;
        article.IsRelevant = structuredResponseDto.IsRelevant;
        article.EstimatedPercentRelevant = structuredResponseDto.EstimatedPercentRelevant;
        article.RelevanceReason = structuredResponseDto.RelevanceReason;
        article.DateProcessed = DateTime.Now;
    }
}
