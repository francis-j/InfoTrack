using System.Net;
using System.Web;
using Domain.Types;
using Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Scraper.Application.Abstractions;

namespace Scraper.Infrastructure.Services;

public class GoogleSearchService
(
    IConfiguration configuration,
    IHttpClientFactory factory,
    [FromKeyedServices(SearchEngine.Google)] ISearchParser parser
) : ISearchService
{
    private record SearchConfiguration(bool Enabled, string UserAgent, string Cookie);

    private readonly ISearchParser _parser = parser;
    private readonly HttpClient _client = factory.CreateClient("Google");
    private readonly SearchConfiguration _configuration =
        configuration.GetSection("Search:Google").Get<SearchConfiguration>() ?? new SearchConfiguration(false, string.Empty, string.Empty);

    public async ValueTask<Search> SearchAsync(Search search)
    {
        if (string.IsNullOrWhiteSpace(search.Url))
        {
            search.SetResult("0");
            return search;
        }

        string url = $"https://www.google.co.uk/search?num=100&q={HttpUtility.UrlEncode(search.Keywords)}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        if (!string.IsNullOrWhiteSpace(_configuration.UserAgent))
            request.Headers.Add(HeaderNames.UserAgent, _configuration.UserAgent);

        request.Headers.Add(HeaderNames.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        request.Headers.Add(HeaderNames.AcceptLanguage, "en-GB,en;q=0.5");
        request.Headers.Add(HeaderNames.AcceptEncoding, "gzip, deflate, br, zstd");
        request.Headers.Add(HeaderNames.Connection, "keep-alive");
        request.Headers.Add(HeaderNames.UpgradeInsecureRequests, "1");
        request.Headers.Add("Sec-Fetch-Dest", "document");
        request.Headers.Add("Sec-Fetch-Mode", "navigate");
        request.Headers.Add("Sec-Fetch-Site", "none");
        request.Headers.Add("Sec-Fetch-User", "?1");
        request.Headers.Add("Priority", "u=0, i");
        request.Headers.Add(HeaderNames.Pragma, "no-cache");
        request.Headers.Add(HeaderNames.CacheControl, "no-cache");
        request.Headers.Add(HeaderNames.TE, "trailers");

        if (!string.IsNullOrWhiteSpace(_configuration.Cookie))
            request.Headers.Add(HeaderNames.Cookie, _configuration.Cookie);

        var response = await _client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Redirect)
        {
            request = new HttpRequestMessage(HttpMethod.Get, response.Headers.Location);
            request.Headers.Add(HeaderNames.Referer, url);

            foreach (string cookie in response.Headers.GetValues("Set-Cookie"))
                request.Headers.Add(HeaderNames.Cookie, cookie);

            request.Headers.Remove("Sec-Fetch-Site");
            request.Headers.Remove("Sec-Fetch-User");
            request.Headers.Add("Sec-Fetch-Site", "same-origin");

            response = await _client.SendAsync(request);
        }

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        var links = _parser.GetLinks(stream);
        var instances = new List<int>();

        for (int i = 0; i < links.Count; i++)
        {
            string link = links[i];
            if (link.Contains(search.Url, StringComparison.OrdinalIgnoreCase))
                instances.Add(i + 1);
        }

        search.SetResult(instances.Count > 0
            ? string.Join(", ", instances)
            : "0");

        return search;
    }
}