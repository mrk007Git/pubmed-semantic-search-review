using PubMedSemanticSearchReview.Application.OpenAi.Responses;

namespace PubMedSemanticSearchReview.Application.OpenAi;

public class UsageDto
{
    public int PromptTokens { get; set; }

    public int CompletionTokens { get; set; }

    public int TotalTokens { get; set; }

    public static UsageDto Create(Usage usage) => new()
    {
        PromptTokens = usage.PromptTokens,
        CompletionTokens = usage.CompletionTokens,
        TotalTokens = usage.TotalTokens
    };
}
