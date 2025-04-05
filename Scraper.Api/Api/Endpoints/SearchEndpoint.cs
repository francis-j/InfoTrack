using Domain.Types;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Scraper.Application.Abstractions;
using Scraper.Application.Configuration;

namespace Scraper.Api.Endpoints;

public static class SearchEndpoint
{
    public static void MapSearchEndpoint(this WebApplication app)
    {
        app.MapGet("/api/search", async (
            [FromServices] SearchConfiguration configuration,
            [FromServices] IServiceProvider provider,
            [FromQuery] SearchEngine searchEngine,
            [FromQuery] string keywords,
            [FromQuery] string url) =>
        {
            if (!Enum.IsDefined(searchEngine))
                return Results.BadRequest("Invalid search engine type.");

            if (!configuration.Enabled)
                searchEngine = SearchEngine.Offline;

            var service = provider.GetKeyedService<ISearchService>(searchEngine);
            if (service is null)
                return Results.InternalServerError("Unable to find service for search engine.");

            var search = await service.SearchAsync(new Search(keywords, url));
            return Results.Ok(search);
        }).AllowAnonymous()
        .CacheOutput(policy => policy
            .SetVaryByQuery("searchEngine", "keywords", "url")
            .Expire(TimeSpan.FromMinutes(5)));
    }
}