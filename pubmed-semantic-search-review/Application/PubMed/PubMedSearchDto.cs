using System.Text.Json;

namespace PubMedSemanticSearchReview.Application.PubMed;

public class PubMedSearchDto(string searchTerm, DateTime? startDate = null, DateTime? endDate = null)
{
    public string SearchTerm { get; set; } = searchTerm;
    public DateTime? StartDate { get; set; } = startDate;
    public DateTime? EndDate { get; set; } = endDate;

    public static PubMedSearchDto Create(string searchTerm, DateTime? startDate = null, DateTime? endDate = null)
    {
        return new PubMedSearchDto(searchTerm, startDate, endDate);
    }

    public static PubMedSearchDto? Create(string json)
    {
        return JsonSerializer.Deserialize<PubMedSearchDto>(json);
    }
}
