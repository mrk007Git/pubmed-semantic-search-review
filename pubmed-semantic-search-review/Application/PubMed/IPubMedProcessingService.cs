namespace PubMedSemanticSearchReview.Application.PubMed;

public interface IPubMedProcessingService
{
    Task ProcessPubMedSearchTermsAsync( string articleOutputSavePath);
}