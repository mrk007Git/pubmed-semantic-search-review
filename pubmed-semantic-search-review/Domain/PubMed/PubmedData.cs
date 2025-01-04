using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class PubmedData
{
    [XmlElement("History")]
    public History? History { get; set; }
}
