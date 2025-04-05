using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Scraper.Application.Abstractions;

namespace Scraper.Application.Parsers;

public class GoogleSearchParser(ILogger<GoogleSearchParser> logger) : ISearchParser
{
    private readonly ILogger<GoogleSearchParser> _logger = logger;

    public List<string> GetLinks(Stream stream)
    {
        var result = new List<string>();
        var document = new HtmlDocument();
        document.Load(stream);

        var errors = document.ParseErrors.ToList();
        if (errors.Count > 0)
        {
            foreach (var error in errors)
            {
                _logger.LogError("An error occurred while parsing the HTML stream. Reason: {Reason}", error.Reason);
            }
            throw new InvalidOperationException("Invalid HTML.");
        }

        _logger.LogInformation("Successfully parsed HTML.");
        _logger.LogTrace("{Html}", document.Text);
        var rso = document.GetElementbyId("rso");
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        // Justification: HtmlAgilityPack does not have NRT enabled
        if (rso is not null)
        {
            foreach (var descendant in rso.Descendants())
            {
                if (!descendant.Attributes.Contains("href"))
                    continue;

                result.Add(descendant.Attributes["href"].Value);
            }
        }

        return result;
    }
}