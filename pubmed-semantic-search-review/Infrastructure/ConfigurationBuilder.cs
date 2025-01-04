using Microsoft.Extensions.Configuration;

namespace PubMedSemanticSearchReview.Infrastructure;

internal class ConfigurationBuilder
{
    public static IConfigurationRoot GetConfiguration(IConfigurationBuilder configurationBuilder) => configurationBuilder
        .AddJsonFile("appsettings.json", optional: true)
        .AddUserSecrets<Program>()
        .Build();
}
