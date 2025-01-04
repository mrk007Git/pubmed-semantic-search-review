using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;

public partial class JsonSchema<TProperties>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("strict")]
    public bool? Strict { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("schema")]
    public Schema<TProperties> Schema { get; set; } = default!;
}
