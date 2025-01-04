
namespace PubMedSemanticSearchReview.Infrastructure
{
    public interface ICsvService<T>
    {
        void WriteCsv(string filePath, IEnumerable<T> records);
    }
}