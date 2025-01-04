using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;

public partial class CustomType
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("description")]
    public string Description { get; set; } = default!;
}
