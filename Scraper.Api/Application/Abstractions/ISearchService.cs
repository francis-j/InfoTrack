using Domain.ValueObjects;

namespace Scraper.Application.Abstractions;

public interface ISearchService
{
    ValueTask<Search> SearchAsync(Search search);
}