using PubMedSemanticSearchReview.Domain.PubMed;

namespace PubMedSemanticSearchReview.Application.PubMed;

public interface IPubmedArticleSetService
{
    PubMedArticleDto? GetPubMedArticleFromXml(string xmlContent);
    PubmedArticleSet? GetPubmedArticleSet(string xmlContent);
}