using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class MeshHeading
{
    [XmlElement("DescriptorName")]
    public string? DescriptorName { get; set; }
}
