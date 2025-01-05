using PubMedSemanticSearchReview.Application.OpenAi;
using PubMedSemanticSearchReview.Application.OpenAi.AbstractAnalysis;
using PubMedSemanticSearchReview.Infrastructure.Configuration;

namespace PubMedSemanticSearchReview.Application.Data;
public static class ArticleDtoExtensions
{
    public static void UpdateWithStructuredOutput(this ArticleDto article, StructuredResponseDto structuredResponseDto, OpenAiConfig.TokenPricingConfig tokenPricingConfig)
    {
        article.AbstractSummary = structuredResponseDto.AbstractSummary;
        article.IsRelevant = structuredResponseDto.IsRelevant;
        article.EstimatedPercentRelevant = structuredResponseDto.EstimatedPercentRelevant;
        article.RelevanceReason = structuredResponseDto.RelevanceReason;
        article.DateProcessed = DateTime.Now;
        article.PromptTokens = structuredResponseDto.Usage.PromptTokens;
        article.CompletionTokens = structuredResponseDto.Usage.CompletionTokens;
        article.EstimatedTotalCost = TokenCalculationService.CalculateTokenCost(structuredResponseDto.Usage, tokenPricingConfig);
    }
}
