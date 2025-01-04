using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class Date
{
    [XmlElement("Year")]
    public int? Year { get; set; }

    [XmlElement("Month")]
    public int? Month { get; set; }

    [XmlElement("Day")]
    public int? Day { get; set; }

    public DateTime? ToDateTime()
    {
        if (Year == null || Month == null || Day == null)
        {
            return null;
        }

        return new DateTime(Year.Value, Month.Value, Day.Value);
    }
}
