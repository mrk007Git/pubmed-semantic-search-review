using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace PubMedSemanticSearchReview.Infrastructure;

public class CsvService<T>(ClassMap<T> classMap) : ICsvService<T>
{
    private readonly ClassMap<T> _classMap = classMap;

    public void WriteCsv(string filePath, IEnumerable<T> records)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap(_classMap);
        csv.WriteRecords(records);
    }
}
