using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubMedSemanticSearchReview.Application.PubMed
{
    internal class PubMedStartupService
    {
        public static string[] PrepareSearchQuery(ILogger logger)
        {
            string[] pubMedSearchTerms = File.ReadAllLines(@"Data\PubMedSearchTerms.txt");

            if (pubMedSearchTerms.Length == 0)
            {
                logger.Error("No PubMed search terms found in Data\\PubMedSearchTerms.txt. Exiting...");
                return [];
            }

            Console.WriteLine($"Are you sure you want to process {string.Join(", ", pubMedSearchTerms)}? (Press ENTER to continue)");

            if (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Console.WriteLine("Exiting...");
                Console.WriteLine();
                return [];
            }

            var duplicateTerms = pubMedSearchTerms
                .GroupBy(term => term)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            if (duplicateTerms.Any())
            {
                logger.Warning("Duplicate search terms found: {DuplicateTerms}. Duplicates will be removed!", string.Join(", ", duplicateTerms));
            }

            return pubMedSearchTerms.Distinct().ToArray();
        }
    }
}
