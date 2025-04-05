using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using Scraper.Application.Parsers;

namespace Scraper.Tests.Parsers;

public class GoogleSearchParserTests
{
    private readonly Mock<ILogger<GoogleSearchParser>> _loggerMock = new();

    [Fact]
    public void GetLinks_WithValidHtml_HasExactly5Links()
    {
        const string html =
            """
            <html>
                <head>
                </head>
                <body>
                    <main>
                        <div id="rso">
                            <div class="foo">
                                <a href="https://site1.com">Site 1</a>
                            </div>
                            <div>
                                <div class="foo">
                                    <a href="https://site2.com">Site 2</a>
                                </div>
                                <div class="foo">
                                    <a href="https://site3.com">Site 3</a>
                                </div>
                                <div class="foo">
                                    <a href="https://site4.com">Site 4</a>
                                </div>
                                <div class="foo">
                                    <a href="https://site5.com">Site 5</a>
                                </div>
                            </div>
                        </div>
                        <div class="foo">
                            <a href="https://site5.com">Site 6</a>
                        </div>
                    </main>
                </body>
                 <footer>
                 </footer>
            </html>
            """;
        var parser = new GoogleSearchParser(_loggerMock.Object);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
        var links = parser.GetLinks(stream);
        Assert.Equal(5, links.Count);
    }

    [Fact]
    public void GetLinks_WithInvalidHtml_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            const string html =
                """
                <html>
                    <body>
                        <main>
                            <div id="page">
                                <div id="content">
                                    <h1>Page</h2>
                                </div>
                            </div>
                        </main>
                    </body>
                </html>
                """;
            var parser = new GoogleSearchParser(_loggerMock.Object);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            _ = parser.GetLinks(stream);
        });
    }
}