using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class History
{
    [XmlElement("PubMedPubDate")]
    public List<PubMedPubDate>? PubMedPubDate { get; set; }
}
