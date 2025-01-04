using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.Requests;

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class Request
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
}