using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.Responses;

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = default!;

    [JsonPropertyName("content")]
    public string Content { get; set; } = default!;
}