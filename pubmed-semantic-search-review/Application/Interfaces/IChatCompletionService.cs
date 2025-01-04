using DhsResearchLibrary.Application.OpenAI;
using AbstractAnalysisStructuredOutputResponseDto = PubMedSemanticSearchReview.Application.OpenAi.AbstractAnalysis.StructuredResponseDto;

namespace DhsResearchLibrary.Infrastructure.OpenAI;

public interface IChatCompletionService
{
    Task<ChatResponseWithUsageDto?> GetChatResponseAsync(string systemPrompt, string userPrompt);
    Task<AbstractAnalysisStructuredOutputResponseDto?> GetStructuredAbstractAnalysisResponseAsync(string systemPrompt, string userPrompt);
}