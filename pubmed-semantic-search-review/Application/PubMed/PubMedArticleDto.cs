using System.Text.Json.Serialization;

namespace PubMedSemanticSearchReview.Application.PubMed;

public class PubMedArticleDto
{
    public long PmId { get; set; }
    public DateTime? DateCompleted { get; set; }
    public DateTime? DateRevised { get; set; }
    public string? JournalName { get; set; }
    public string? ArticleTitle { get; set; }
    public string? AbstractText { get; set; }

    [JsonIgnore]
    public bool HasAbstract => !string.IsNullOrEmpty(AbstractText);
    public string? SearchTerm { get; set; }

    public static PubMedArticleDto Create(long pmId, DateTime? dateCompleted, DateTime? dateRevised,
        string? journalName, string? articleTitle, string? abstractText, string? searchTerm)
    {
        return new PubMedArticleDto
        {
            PmId = pmId,
            DateCompleted = dateCompleted,
            DateRevised = dateRevised,
            JournalName = journalName,
            ArticleTitle = articleTitle,
            AbstractText = abstractText,
            SearchTerm = searchTerm
        };
    }

    public override string ToString()
    {
        return $"PMID: {PmId}\r\nDate Completed: {DateCompleted}\r\nDate Revised: {DateRevised}\r\nJournal Name: {JournalName}\r\nArticle Title: {ArticleTitle}\r\nAbstract:\r\n{AbstractText}\r\nSearch Term: {SearchTerm}";
    }
}
