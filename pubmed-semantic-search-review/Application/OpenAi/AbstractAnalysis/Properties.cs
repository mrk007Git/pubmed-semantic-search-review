using System.Text.Json.Serialization;
using PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;

namespace PubMedSemanticSearchReview.Application.OpenAi.AbstractAnalysis;

public partial class Properties
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("is_relevant")]
    public CustomType IsRelevant { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("estimated_percent_relevant")]
    public CustomType EstimatedPercentRelevant { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("abstract_summary")]
    public CustomType AbstractSummary { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("relevance_reason")]
    public CustomType RelevanceReason { get; set; } = default!;
}
