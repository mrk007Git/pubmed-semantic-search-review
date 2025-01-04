using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class Article
{
    [XmlElement("ArticleTitle")]
    public string? ArticleTitle { get; set; }

    [XmlElement("Abstract")]
    public Abstract? Abstract { get; set; }

    [XmlElement("Journal")]
    public Journal? Journal { get; set; }
}
