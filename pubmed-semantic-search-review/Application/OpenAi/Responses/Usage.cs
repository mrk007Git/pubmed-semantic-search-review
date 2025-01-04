using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.Responses;

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; } = 13;

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; } = 7;

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; } = 20;
}