namespace PubMedSemanticSearchReview.Infrastructure.Configuration;

public class PubMedConfig
{
    public string BaseAddress { get; set; } = default!;

    public string BaseArticleUrl { get; set; } = default!;

    public string ApiKey { get; set; } = default!;

    public int RateLimitPerSecond { get; set; }

    public EndpointsConfig Endpoints { get; set; } = default!;

    public class EndpointsConfig
    {
        public string Fetch { get; set; } = default!;
        public string Summary { get; set; } = default!;
        public string Search { get; set; } = default!;
    }
}
