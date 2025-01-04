using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class PubMedPubDate
{
    [XmlAttribute("PubStatus")]
    public string? PubStatus { get; set; }

    [XmlElement("Year")]
    public string? Year { get; set; }

    [XmlElement("Month")]
    public string? Month { get; set; }

    [XmlElement("Day")]
    public string? Day { get; set; }
}
