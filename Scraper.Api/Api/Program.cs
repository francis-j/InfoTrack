using System.Net;
using Scraper.Api.Endpoints;
using Scraper.Application;
using Scraper.Application.Configuration;
using Scraper.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpClientDefaults(configure =>
{
    configure.ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler()
    {
        AllowAutoRedirect = false,
        AutomaticDecompression = DecompressionMethods.All,
        CookieContainer = new CookieContainer(),
        UseCookies = true
    });
});
builder.Services.AddHttpClient();

builder.Services.AddSingleton<SearchConfiguration>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var searchConfig = configuration.GetSection("Search:Google").Get<SearchConfiguration>();
    return searchConfig ?? new SearchConfiguration(false, string.Empty, string.Empty);
});
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddOpenApi();

builder.Services.AddCors(configure =>
{
    configure.AddDefaultPolicy(o => o
        //.WithOrigins("http://localhost:5173")
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseHttpsRedirection();

app.MapSearchEndpoint();

app.Run();
