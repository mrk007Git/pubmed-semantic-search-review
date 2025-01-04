using System.Text.Json.Serialization;
using PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;

namespace PubMedSemanticSearchReview.Application.OpenAi.AbstractAnalysis;

public class StructuredResponseDto : StructuredResponseBaseDto
{
    [JsonPropertyName("is_relevant")]
    public bool IsRelevant { get; set; }
    [JsonPropertyName("estimated_percent_relevant")]
    public decimal? EstimatedPercentRelevant { get; set; }
    [JsonPropertyName("abstract_summary")]
    public string? AbstractSummary { get; set; }
    [JsonPropertyName("relevance_reason")]
    public string? RelevanceReason { get; set; }
}
