namespace Scraper.Application.Abstractions;

public interface ISearchParser
{
    List<string> GetLinks(Stream stream);
}