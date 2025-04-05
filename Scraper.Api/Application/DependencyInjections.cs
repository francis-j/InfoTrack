using Domain.Types;
using Microsoft.Extensions.DependencyInjection;
using Scraper.Application.Abstractions;
using Scraper.Application.Parsers;

namespace Scraper.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<ISearchParser, GoogleSearchParser>(SearchEngine.Google);

        return services;
    }
}