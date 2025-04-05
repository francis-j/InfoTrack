using Domain.Types;
using Microsoft.Extensions.DependencyInjection;
using Scraper.Application.Abstractions;
using Scraper.Infrastructure.Services;

namespace Scraper.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<ISearchService, OfflineSearchService>(SearchEngine.Offline);
        services.AddKeyedScoped<ISearchService, GoogleSearchService>(SearchEngine.Google);

        return services;
    }
}