using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class MedlineCitation
{
    [XmlElement("PMID")]
    public string? PMID { get; set; }

    [XmlElement("Article")]
    public Article? Article { get; set; }

    [XmlElement("DateCompleted")]
    public Date? DateCompleted { get; set; }

    [XmlElement("DateRevised")]
    public Date? DateRevised { get; set; }

    [XmlElement("MeshHeadingList")]
    public List<MeshHeading>? MeshHeadingList { get; set; }
}
