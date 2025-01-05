using PubMedSemanticSearchReview.Infrastructure.Configuration;

namespace PubMedSemanticSearchReview.Application.OpenAi;

internal class TokenCalculationService
{
    public static double CalculateTokenCost(UsageDto usage, OpenAiConfig.TokenPricingConfig pricing)
    {
        var promptCost = CalculateTokenCost(usage.PromptTokens, pricing.PromptTokensUsdPerMillion);
        var completionCost = CalculateTokenCost(usage.CompletionTokens, pricing.CompletionTokensUsdPerMillion);

        return promptCost + completionCost;
    }

    private static double CalculateTokenCost(int tokens, double tokenCostPerMillion) => tokens * tokenCostPerMillion / 1000000.0;
}
