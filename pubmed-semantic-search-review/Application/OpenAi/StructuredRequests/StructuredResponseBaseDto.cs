namespace PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;

public abstract class StructuredResponseBaseDto
{
    public UsageDto Usage { get; set; } = new UsageDto();
}
