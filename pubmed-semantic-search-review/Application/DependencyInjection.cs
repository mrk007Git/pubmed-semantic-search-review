using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PubMedSemanticSearchReview.Application.Data;
using PubMedSemanticSearchReview.Application.PubMed;
using PubMedSemanticSearchReview.Infrastructure;
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

        services.AddSingleton<ICsvService<ArticleDto>>(provider => new CsvService<ArticleDto>(new ArticleDtoCsvMap()));

        return services;
    }
}
