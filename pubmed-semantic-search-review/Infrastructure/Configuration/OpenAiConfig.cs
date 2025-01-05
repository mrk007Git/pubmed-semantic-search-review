namespace PubMedSemanticSearchReview.Infrastructure.Configuration;

public class OpenAiConfig
{
    public string BaseAddress { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
    public string SystemRole { get; set; } = default!;
    public double Temperature { get; set; }
    public int MaxTokens { get; set; }
    public EndpointsConfig Endpoints { get; set; } = default!;
    public TokenPricingConfig TokenPricing { get; set; } = default!;

    public class TokenPricingConfig
    {
        public double PromptTokensUsdPerMillion { get; set; }
        public double CompletionTokensUsdPerMillion { get; set; }
    }

    public class EndpointsConfig
    {
        public string ChatCompletions { get; set; } = default!;
    }
}
