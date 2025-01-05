using CsvHelper.Configuration;

namespace PubMedSemanticSearchReview.Application.Data;

internal class ArticleDtoCsvMap : ClassMap<ArticleDto>
{
    public ArticleDtoCsvMap()
    {
        Map(m => m.SearchTerm).Name("Search Term");
        Map(m => m.PmId).Name("PMID");
        Map(m => m.DateComplete).Name("Date Completed");
        Map(m => m.DateRevised).Name("Date Revised");
        Map(m => m.JournalName).Name("Journal Name");
        Map(m => m.ArticleTitle).Name("Article Title");
        Map(m => m.AbstractText).Name("Abstract Text");
        Map(m => m.IsRelevant).Name("Is Relevant");
        Map(m => m.EstimatedPercentRelevant).Name("Estimated Percent Relevant");
        Map(m => m.AbstractSummary).Name("Abstract Summary");
        Map(m => m.RelevanceReason).Name("Relevance Reason");
        Map(m => m.DateProcessed).Name("Date Processed");
        Map(m => m.PromptTokens).Name("Prompt Tokens");
        Map(m => m.CompletionTokens).Name("Completion Tokens");
        Map(m => m.EstimatedTotalCost).Name("Estimated Total Cost");
        Map(m => m.ArticleUrl).Name("Article URL");
    }
}
