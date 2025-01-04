using System.Net.Http.Json;
using System.Text.Json;
using DhsResearchLibrary.Application.OpenAI;
using Microsoft.Extensions.Options;
using PubMedSemanticSearchReview.Application.OpenAi;
using PubMedSemanticSearchReview.Application.OpenAi.AbstractAnalysis;
using PubMedSemanticSearchReview.Application.OpenAi.Requests;
using PubMedSemanticSearchReview.Application.OpenAi.Responses;
using PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;
using PubMedSemanticSearchReview.Infrastructure.Configuration;
using Serilog;
using AbstractAnalysisProperties = PubMedSemanticSearchReview.Application.OpenAi.AbstractAnalysis.Properties;
using RequestMessage = PubMedSemanticSearchReview.Application.OpenAi.Requests.Message;

namespace DhsResearchLibrary.Infrastructure.OpenAI;

public class ChatCompletionService(HttpClient httpClient, IOptions<OpenAiConfig> options, ILogger logger) : IChatCompletionService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly OpenAiConfig _config = options.Value;
    private readonly ILogger _logger = logger;

    public async Task<ChatResponseWithUsageDto?> GetChatResponseAsync(string systemPrompt, string userPrompt)
    {
        var request = new Request
        {
            Messages =
            [
                new RequestMessage{Role = "system",
                Content = systemPrompt},
                new RequestMessage{ Role = "user",
                Content = userPrompt}
            ],
            Model = _config.Model,
            Temperature = _config.Temperature
        };

        _httpClient.DefaultRequestHeaders.Authorization =
           new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _config.ApiKey);
        var httpResponseMessage = await _httpClient.PostAsJsonAsync(_config.Endpoints.ChatCompletions, request);

        httpResponseMessage.EnsureSuccessStatusCode();

        var response = await httpResponseMessage.Content.ReadFromJsonAsync<Response>();

        var message = response?.Choices.FirstOrDefault()?.Message.Content;
        var usage = response?.Usage;

        if (message == null || usage == null)
        {
            _logger.Warning("ChatCompletionService.GetChatResponseAsync: message or usage is null");
            return null;
        }

        return new ChatResponseWithUsageDto(message, usage);
    }

    public async Task<StructuredResponseDto?> GetStructuredAbstractAnalysisResponseAsync(string systemPrompt, string userPrompt)
    {
        StructuredRequestDto<AbstractAnalysisProperties> request =
            StructureRequestBuilder.GetAbstractAnalysisRequest(_config.Model, systemPrompt, userPrompt, _config.MaxTokens, _config.Temperature);

        _httpClient.DefaultRequestHeaders.Authorization =
         new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _config.ApiKey);
        var httpResponseMessage = await _httpClient.PostAsJsonAsync(_config.Endpoints.ChatCompletions, request);

        httpResponseMessage.EnsureSuccessStatusCode();

        var response = await httpResponseMessage.Content.ReadFromJsonAsync<Response>();

        if (response == null)
        {
            _logger.Error("ChatCompletionService.GetStructuredAbstractAnalysisResponseAsync: response is null");
            throw new InvalidOperationException("Response is null");
        }

        var jsonResponse = response.Choices.FirstOrDefault()?.Message.Content;

        if (jsonResponse == null)
        {
            _logger.Error("ChatCompletionService.GetStructuredAbstractAnalysisResponseAsync: jsonResponse is null");
            throw new InvalidOperationException("jsonResponse is null");
        }

        if (!string.IsNullOrEmpty(jsonResponse))
        {
            StructuredResponseDto? result = JsonSerializer.Deserialize<StructuredResponseDto>(jsonResponse);
            return result;
        }

        _logger.Warning("ChatCompletionService.GetStructuredAbstractAnalysisResponseAsync: jsonResponse is empty");

        return null;
    }
}
