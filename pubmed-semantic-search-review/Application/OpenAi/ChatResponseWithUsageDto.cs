using PubMedSemanticSearchReview.Application.OpenAi;
using PubMedSemanticSearchReview.Application.OpenAi.Responses;

namespace DhsResearchLibrary.Application.OpenAI;

public class ChatResponseWithUsageDto
{
    public ChatResponseWithUsageDto(string message, int promptTokens, int completionTokens)
    {
        Message = message;
        Usage = new UsageDto
        {
            PromptTokens = promptTokens,
            CompletionTokens = completionTokens,
            TotalTokens = promptTokens + completionTokens
        };
    }

    public ChatResponseWithUsageDto(string message, Usage usage)
    {
        Message = message;
        Usage = UsageDto.Create(usage);
    }
    public string Message { get; set; }

    public UsageDto Usage { get; set; }
}
