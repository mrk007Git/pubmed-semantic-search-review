using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.Responses;

public class Response
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("@object")]
    public string Object { get; set; } = default!;

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = default!;

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; } = new Usage();

    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = new List<Choice>();
}
