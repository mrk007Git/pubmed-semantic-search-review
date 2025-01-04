namespace PubMedSemanticSearchReview.Application.Interfaces;

public interface IPubMedService
{
    Task<string> GetFullXmlAsync(long pmid);
    Task<List<long>> BasicSearchAsync(string term, DateTime? startDate = null, DateTime? endDate = null);
    Task<string> FetchAsync(long id);
    List<long> GetIdsFromXml(string xml);
}
