using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class PubmedArticle
{
    [XmlElement("MedlineCitation")]
    public MedlineCitation? MedlineCitation { get; set; }

    [XmlElement("PubmedData")]
    public PubmedData? PubmedData { get; set; }
}
