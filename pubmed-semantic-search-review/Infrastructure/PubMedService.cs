using Serilog;
using Microsoft.Extensions.Options;
using PubMedSemanticSearchReview.Application.Interfaces;
using PubMedSemanticSearchReview.Infrastructure.Configuration;
using System.Xml.Linq;

namespace PubMedSemanticSearchReview.Infrastructure;

public class PubMedService(HttpClient httpClient, IOptions<PubMedConfig> config, ILogger logger) : IPubMedService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly ILogger _logger = logger;
    private readonly PubMedConfig _config = config?.Value ?? throw new ArgumentNullException(nameof(config));

    public async Task<string?> GetFullTextArticleLinkAsync(long pmid)
    {
        var xmlContent = await GetFullXmlAsync(pmid).ConfigureAwait(false);
        var xmlDocument = XDocument.Parse(xmlContent);

        var articleLink = xmlDocument.Descendants("ELocationID")
                                     .FirstOrDefault(el => el.Attribute("EIdType")?.Value == "doi")?.Value;

        return !string.IsNullOrEmpty(articleLink) ? $"https://doi.org/{articleLink}" : null;
    }

    public async Task<string> GetFullXmlAsync(long pmid)
    {
        var requestUrl = $"{_config.Endpoints.Fetch}?db=pubmed&id={pmid}&retmode=xml&api_key={_config.ApiKey}";

        var response = await _httpClient.GetAsync(requestUrl).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    public async Task<List<long>> BasicSearchAsync(string term, DateTime? startDate = null, DateTime? endDate = null)
    {
        List<long> allIds = [];
        int retStart = 0;
        int retMax = 1000; // Adjust based on your API's settings
        int totalCount = 0; // This is reset on the first fetch

        var sanitizedTerm = SanitizeTerm(term);

        do
        {
            // Build the query URL
            var url = BuildQueryUrl(term, retStart, retMax, startDate, endDate);
            var xml = await FetchXmlResponseAsync(url);

            // Get the total count of results from the first response
            if (retStart == 0)
            {
                totalCount = GetCount(xml);
            }

            // Extract IDs from the current XML response and add them to the list
            var ids = GetIdsFromXml(xml);
            allIds.AddRange(ids);

            // Increment retStart to fetch the next set of results
            retStart += retMax;

        } while (retStart < totalCount);

        return allIds;
    }

    private string SanitizeTerm(string term)
    {
        return string.Concat(term.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
    }

    private string BuildQueryUrl(string term, int retStart, int retMax, DateTime? startDate, DateTime? endDate)
    {
        var dateFilter = startDate.HasValue && endDate.HasValue
            ? $"+AND+(\"{startDate:yyyy/MM/dd}\"[PDAT]:\"{endDate:yyyy/MM/dd}\"[PDAT])"
            : string.Empty;

        return $"{_config.Endpoints.Search}?db=pubmed&term={term}[Title/Abstract]{dateFilter}" +
               $"&retstart={retStart}&retmax={retMax}&api_key={_config.ApiKey}";
    }

    private async Task<string> FetchXmlResponseAsync(string url)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            await Task.Delay((1000 / _config.RateLimitPerSecond) + 1);

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(ex, "An error occurred while fetching XML response from {Url}", url);
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An unexpected error occurred while fetching XML response from {Url}", url);
            throw;
        }
    }

    public List<long> GetIdsFromXml(string xml)
    {
        XDocument doc = XDocument.Parse(xml);

        return doc.Descendants("IdList")
                            .Elements("Id")
                            .Select(x => long.Parse(x.Value))
                            .ToList();
    }

    private int GetCount(string xml)
    {
        XDocument doc = XDocument.Parse(xml);

        var countElement = doc.Descendants("Count").FirstOrDefault();

        return countElement != null ? int.Parse(countElement.Value) : 0;
    }

    public async Task<string> FetchAsync(long id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_config.Endpoints.Fetch}?&id={id}&api_key={_config.ApiKey}");
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
