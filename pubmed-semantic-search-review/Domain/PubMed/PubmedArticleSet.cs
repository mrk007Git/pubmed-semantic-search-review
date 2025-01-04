using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

[XmlRoot("PubmedArticleSet")]
public class PubmedArticleSet
{
    [XmlElement("PubmedArticle")]
    public List<PubmedArticle>? PubmedArticles { get; set; }
}