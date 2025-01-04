using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class AbstractText
{
    [XmlAttribute("Label")]
    public string? Label { get; set; }

    [XmlAttribute("NlmCategory")]
    public string? NlmCategory { get; set; }

    [XmlText]
    public string? Text { get; set; }
}
