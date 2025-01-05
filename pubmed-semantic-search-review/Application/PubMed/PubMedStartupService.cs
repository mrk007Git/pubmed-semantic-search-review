using Serilog;
using System.Text.Json;

namespace PubMedSemanticSearchReview.Application.PubMed;

internal class PubMedStartupService
{
    public static List<PubMedSearchDto> PrepareSearchQuery(ILogger logger)
    {
        var json = File.ReadAllText("Data\\PubMedSearchTerms.json");

        if (string.IsNullOrEmpty(json))
        {
            logger.Error("No PubMed search terms found in Data\\PubMedSearchTerms.json. Exiting...");
            return [];
        }

        var pubMedSearchTerms = JsonSerializer.Deserialize<List<PubMedSearchDto>>(json);

        if (pubMedSearchTerms == null || pubMedSearchTerms.Count == 0)
        {
            logger.Error("No PubMed search terms found in Data\\PubMedSearchTerms.json. Exiting...");
            return [];
        }

        Console.WriteLine($"Are you sure you want to process {string.Join(", ", pubMedSearchTerms.Select(term => term.SearchTerm))}? (Press ENTER to continue)");

        if (Console.ReadKey().Key != ConsoleKey.Enter)
        {
            Console.WriteLine("Exiting...");
            Console.WriteLine();
            return [];
        }

        var duplicateTerms = pubMedSearchTerms
            .GroupBy(term => term.SearchTerm)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicateTerms.Count != 0)
        {
            logger.Warning("Duplicate search terms found: {DuplicateTerms}. Duplicates will be removed!", string.Join(", ", duplicateTerms));
        }

        return pubMedSearchTerms.DistinctBy(term => term.SearchTerm).ToList();
    }
}
