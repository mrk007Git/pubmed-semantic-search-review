using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.Requests;

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = default!;

    [JsonPropertyName("content")]
    public string Content { get; set; } = default!;
}

public class Request
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = default!;

    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; } = default!;

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
}