using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PubMedSemanticSearchReview.Application.PubMed;
using Serilog;

namespace PubMedSemanticSearchReview.Application;

internal static class DependencyInjection
{
    public static ServiceCollection AddApplication(this ServiceCollection services, Serilog.ILogger logger)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(logger, dispose: true);
        });

        services.AddSingleton(Log.Logger);

        services.AddTransient<IPubmedArticleSetService, PubmedArticleSetService>();
        services.AddTransient<IPubMedProcessingService, PubMedProcessingService>();

        return services;
    }
}
