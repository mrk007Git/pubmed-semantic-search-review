using DhsResearchLibrary.Infrastructure.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using PubMedSemanticSearchReview.Application.Interfaces;
using PubMedSemanticSearchReview.Infrastructure.Configuration;
using Serilog;

namespace PubMedSemanticSearchReview.Infrastructure;

internal static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        // Add PubMed HTTP Client
        services.AddHttpClientWithConfig<IPubMedService, PubMedService, PubMedConfig>(
            configuration, nameof(PubMedConfig), logger);

        // Add OpenAI HTTP Client
        services.AddHttpClientWithConfig<IChatCompletionService, ChatCompletionService, OpenAiConfig>(
            configuration, nameof(OpenAiConfig), logger);

        // Add Options and Validations
        AddValidatedOptions<PubMedConfig, PubMedConfigValidation>(services, configuration, nameof(PubMedConfig));
        AddValidatedOptions<OpenAiConfig, OpenAiConfigValidation>(services, configuration, nameof(OpenAiConfig));

        services.AddScoped(typeof(ICsvService<>), typeof(CsvService<>));

        return services;
    }

    private static void AddValidatedOptions<TOptions, TValidator>(
        IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
        where TOptions : class
        where TValidator : class, IValidateOptions<TOptions>
    {
        services.AddOptions<TOptions>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<TOptions>, TValidator>();
    }

    private static IServiceCollection AddHttpClientWithConfig<TInterface, TImplementation, TConfig>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName,
        ILogger logger)
        where TInterface : class
        where TImplementation : class, TInterface
        where TConfig : class
    {
        services.AddHttpClient<TInterface, TImplementation>(client =>
        {
            var config = configuration.GetSection(sectionName).Get<TConfig>();
            if (config == null || string.IsNullOrWhiteSpace(GetBaseAddress(config)))
            {
                logger.Error($"{sectionName} section is missing or invalid in the configuration.");
                throw new InvalidOperationException($"{sectionName} section is missing or invalid in the configuration.");
            }
            client.BaseAddress = new Uri(GetBaseAddress(config));
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .AddPolicyHandler(GetRetryPolicy(logger));

        return services;
    }

    private static string GetBaseAddress<TConfig>(TConfig config) where TConfig : class
    {
        return config switch
        {
            PubMedConfig pubMedConfig => pubMedConfig.BaseAddress,
            OpenAiConfig openAiConfig => openAiConfig.BaseAddress,
            _ => throw new ArgumentException($"Unsupported configuration type: {typeof(TConfig).Name}")
        };
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger) => HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound ||
                             msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                6,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    if (outcome.Result?.StatusCode == System.Net.HttpStatusCode.TooManyRequests &&
                        outcome.Result.Headers.RetryAfter != null)
                    {
                        timespan = outcome.Result.Headers.RetryAfter.Delta ?? TimeSpan.FromSeconds(60);
                    }

                    logger.Warning("Retry {retryAttempt} after {totalSeconds} seconds due to {statusCode}", retryAttempt, timespan.TotalSeconds, outcome.Result?.StatusCode);
                });
}
