using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using Domain.Types;
using Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Scraper.Application.Abstractions;
using Scraper.Application.Configuration;

namespace Scraper.Infrastructure.Services;

public class OfflineSearchService
(
    [FromKeyedServices(SearchEngine.Google)] ISearchParser parser
) : ISearchService
{
    private readonly ISearchParser _parser = parser;

    public async ValueTask<Search> SearchAsync(Search search)
    {
        if (string.IsNullOrWhiteSpace(search.Url))
        {
            search.SetResult("0");
            return search;
        }

        await using var stream = Assembly.GetEntryAssembly()?.GetManifestResourceStream("Scraper.Api.Resources.offline_data.html");
        var links = stream is not null
            ? _parser.GetLinks(stream)
            : [];

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