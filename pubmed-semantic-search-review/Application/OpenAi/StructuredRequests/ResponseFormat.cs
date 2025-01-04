using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;

public partial class ResponseFormat<TProperties>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("json_schema")]
    public JsonSchema<TProperties> JsonSchema { get; set; }
}
