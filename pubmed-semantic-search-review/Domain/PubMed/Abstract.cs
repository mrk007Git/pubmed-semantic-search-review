﻿using System.Text;
using System.Xml.Serialization;

namespace PubMedSemanticSearchReview.Domain.PubMed;

public class Abstract
{
    [XmlElement("AbstractText")]
    public List<AbstractText>? AbstractText { get; set; }

    public override string ToString()
    {
        if (AbstractText == null)
        {
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder();
        foreach (var item in AbstractText)
        {
            sb.AppendLine($"{item.Label}\r\n{item.Text}");
        }

        return sb.ToString();
    }
}
