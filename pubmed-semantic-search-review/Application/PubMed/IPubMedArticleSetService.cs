using PubMedSemanticSearchReview.Domain.PubMed;

namespace PubMedSemanticSearchReview.Application.PubMed;

public interface IPubmedArticleSetService
{
    PubMedArticleDto? GetPubMedArticleFromXml(string xmlContent, string searchTerm);
    PubmedArticleSet? GetPubmedArticleSet(string xmlContent);
}