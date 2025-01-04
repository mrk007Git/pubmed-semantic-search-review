using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.Responses;

public class Choice
{
    [JsonPropertyName("message")]
    public Message Message { get; set; } = new Message();

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = "stop";

    [JsonPropertyName("index")]
    public int Index { get; set; } = 0;
}