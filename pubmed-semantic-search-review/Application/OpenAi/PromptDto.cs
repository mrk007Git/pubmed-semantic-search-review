namespace PubMedSemanticSearchReview.Application.OpenAi;

internal class PromptDto(string systemPrompt, string userPrompt)
{
    public string SystemPrompt { get; set; } = systemPrompt;
    public string UserPrompt { get; set; } = userPrompt;
}
