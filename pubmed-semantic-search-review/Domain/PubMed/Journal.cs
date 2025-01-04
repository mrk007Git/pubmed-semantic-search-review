using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class Journal
{
    [XmlElement("Title")]
    public string? Title { get; set; }

    [XmlElement("ISOAbbreviation")]
    public string? ISOAbbreviation { get; set; }
}
