using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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


        return services;
    }
}
