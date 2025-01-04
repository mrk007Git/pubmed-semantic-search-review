namespace PubMedSemanticSearchReview.Infrastructure.Configuration;

public class PubMedConfig
{
    public string BaseAddress { get; set; }
    public string ApiKey { get; set; }

    public int RateLimitPerSecond { get; set; }

    public EndpointsConfig Endpoints { get; set; }

    public class EndpointsConfig
    {
        public string Fetch { get; set; }
        public string Summary { get; set; }
        public string Search { get; set; }
    }
}
