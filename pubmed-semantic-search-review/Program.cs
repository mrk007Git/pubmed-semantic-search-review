﻿using DhsResearchLibrary.Infrastructure.OpenAI;
using Microsoft.Extensions.DependencyInjection;
using PubMedSemanticSearchReview.Application;
using PubMedSemanticSearchReview.Application.Data;
using PubMedSemanticSearchReview.Application.Interfaces;
using PubMedSemanticSearchReview.Application.OpenAi;
using PubMedSemanticSearchReview.Application.PubMed;
using PubMedSemanticSearchReview.Infrastructure;
using Serilog;
using PubMedConfigurationBuilder = PubMedSemanticSearchReview.Infrastructure.ConfigurationBuilder;

Console.WriteLine("Welcome to the PubMed Semantic Search Review!");

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("WARNING: Unchecked searches can lead to excessive consumption costs. This tool has no price controls built in. Refer to the current OpenAI pricing.");
Console.WriteLine("Press 'Enter' to proceed.");
var key = Console.ReadKey().Key;
if (key != ConsoleKey.Enter)
{
    Console.WriteLine("Exiting...");
    Console.ResetColor();
    return;
}
Console.ResetColor();

var configuration = PubMedConfigurationBuilder.GetConfiguration(new Microsoft.Extensions.Configuration.ConfigurationBuilder());

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var articleOutputSavePath = configuration["ArticleOutputSavePath"];

if (string.IsNullOrEmpty(articleOutputSavePath))
{
    logger.Error("ArticleOutputSavePath is not found in the configuration. Please ensure the key 'ArticleOutputSavePath' is defined in the configuration file (e.g., appsettings.json) and has a valid non-empty directory name. Exiting...");
    return;
}

var services = new ServiceCollection();
services.AddInfrastructure(configuration, logger);
services.AddApplication(logger);

var serviceProvider = services.BuildServiceProvider();

var pubMedProcessingService = serviceProvider.GetRequiredService<IPubMedProcessingService>();

await pubMedProcessingService.ProcessPubMedSearchTermsAsync(articleOutputSavePath);
