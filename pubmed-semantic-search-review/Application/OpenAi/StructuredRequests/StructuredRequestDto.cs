using PubMedSemanticSearchReview.Application.OpenAi.Requests;
using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;

public class StructuredRequestDto<TProperties>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("model")]
    public string Model { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("response_format")]
    public ResponseFormat<TProperties> ResponseFormat { get; set; } = default!;
}