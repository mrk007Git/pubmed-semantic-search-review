using PubMedSemanticSearchReview.Domain.PubMed;
using Serilog;

namespace PubMedSemanticSearchReview.Application.PubMed;

public class PubmedArticleSetService : IPubmedArticleSetService
{
    private readonly ILogger _logger;

    public PubmedArticleSetService(ILogger logger)
    {
        _logger = logger;
    }

    public PubMedArticleDto? GetPubMedArticleFromXml(string xmlContent, string searchTerm)
    {
        var pubmedArticleSet = GetPubmedArticleSet(xmlContent);
        if (pubmedArticleSet == null || pubmedArticleSet.PubmedArticles == null || pubmedArticleSet.PubmedArticles.Count == 0)
        {
            return null;
        }

        var pubmedArticle = pubmedArticleSet?.PubmedArticles[0];

        var articleAbstract = pubmedArticle?.MedlineCitation?.Article?.Abstract;

        if (articleAbstract?.AbstractText?.Count == 0)
        {
            return null;
        }
        else
        {
            var medlineCitation = pubmedArticle?.MedlineCitation;

            if (medlineCitation == null || string.IsNullOrEmpty(medlineCitation.PMID))
            {
                return null;
            }

            return PubMedArticleDto.Create(
                                   long.Parse(medlineCitation.PMID),
                                   medlineCitation.DateCompleted?.ToDateTime(),
                                   medlineCitation.DateRevised?.ToDateTime(),
                                   medlineCitation.Article?.Journal?.Title,
                                   medlineCitation.Article?.ArticleTitle,
                                   articleAbstract?.ToString(), searchTerm);

        }
    }

    public PubmedArticleSet? GetPubmedArticleSet(string xmlContent)
    {
        return ExtractPubmedArticleSetFromXml(xmlContent);
    }

    private PubmedArticleSet? ExtractPubmedArticleSetFromXml(string xmlContent)
    {
        if (string.IsNullOrEmpty(xmlContent))
        {
            _logger.Error("xmlContent is null or empty");
            return null;
        }

        _logger.Information("Deserializing PubmedArticleSet...");

        PubmedArticleSet? pubmedArticleSet = null;
        try
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(PubmedArticleSet));
            using var reader = new StringReader(xmlContent);

            if (reader == null)
            {
                return null;
            }

            pubmedArticleSet = (PubmedArticleSet?)serializer.Deserialize(reader);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error deserializing PubmedArticleSet");
        }

        return pubmedArticleSet;
    }
}