using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;

public partial class Schema<TProperties>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("properties")]
    public TProperties Properties { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("required")]
    public List<string> SchemaRequired { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("additionalProperties")]
    public bool? AdditionalProperties { get; set; }
}
