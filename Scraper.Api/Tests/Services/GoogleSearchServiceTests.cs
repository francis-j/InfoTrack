using System.Net;
using Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Moq;
using RichardSzalay.MockHttp;
using Scraper.Application.Abstractions;
using Scraper.Infrastructure.Services;
using Scraper.Tests.Data;

namespace Scraper.Tests.Services;

public class GoogleSearchServiceTests
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder().Build();
    private readonly Mock<IHttpClientFactory> _clientFactoryMock = new();
    private readonly Mock<ISearchParser> _searchParser = new();
    private readonly MockHttpMessageHandler _handler = new();

    [Theory]
    [InlineData("land registry searches", "www.infotrack.co.uk")]
    [InlineData("land registry searches", "www.gov.uk")]
    [InlineData("land registry searches", "landregistry.data.gov.uk")]
    [InlineData("infotrack", "www.infotrack.co.uk")]
    [InlineData("", "www.infotrack.co.uk")]
    public async Task Search_WithValidUrl_ReturnsResult(string keywords, string url)
    {
        _handler.When($"https://www.google.co.uk/search?num=100&q={keywords}")
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(SearchServiceTestData.LandRegistryHtmlResponse)
            });
        _clientFactoryMock.Setup(x => x.CreateClient("Google"))
            .Returns(_handler.ToHttpClient());
        _searchParser.Setup(x => x.GetLinks(It.IsAny<Stream>()))
            .Returns(["www.infotrack.co.uk", "www.gov.uk", "landregistry.data.gov.uk"]);

        var service = new GoogleSearchService(_configuration, _clientFactoryMock.Object, _searchParser.Object);
        var search = await service.SearchAsync(new Search(keywords, url));
        Assert.True(search.Result.Length > 0);
    }

    [Theory]
    [InlineData("land registry searches", "")]
    [InlineData("land registry searches", "   ")]
    [InlineData("land registry searches", "www,infotrack.co.uk")]
    [InlineData("land registry searches", "noreply@infotrack.co.uk")]
    public async Task Search_WithInvalidUrl_ReturnsZero(string keywords, string url)
    {
        _handler.When($"https://www.google.co.uk/search?num=100&q={keywords}")
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(SearchServiceTestData.LandRegistryHtmlResponse)
            });
        _clientFactoryMock.Setup(x => x.CreateClient("Google"))
            .Returns(_handler.ToHttpClient());
        _searchParser.Setup(x => x.GetLinks(It.IsAny<Stream>()))
            .Returns(["www.infotrack.co.uk", "www.gov.uk", "landregistry.data.gov.uk"]);

        var service = new GoogleSearchService(_configuration, _clientFactoryMock.Object, _searchParser.Object);
        var search = await service.SearchAsync(new Search(keywords, url));

        Assert.Equal("0", search.Result);
    }
}