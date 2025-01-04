namespace PubMedSemanticSearchReview.Infrastructure.Configuration;

public class OpenAiConfig
{
    public string BaseAddress { get; set; }
    public string Model { get; set; }
    public string ApiKey { get; set; }
    public string SystemRole { get; set; }
    public int Temperature { get; set; }
    public int MaxTokens { get; set; }
    public EndpointsConfig Endpoints { get; set; }

    public class EndpointsConfig
    {
        public string ChatCompletions { get; set; }
    }
}
