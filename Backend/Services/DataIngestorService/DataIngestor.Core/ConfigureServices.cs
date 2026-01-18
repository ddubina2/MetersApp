using DataIngestor.Core.Interfaces;
using DataIngestor.Core.Options;
using DataIngestor.Core.Services;
using DataIngestor.Core.Services.WeakAppApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataIngestor.Core;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureCoreServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<WeakAppOptions>(configuration.GetSection(nameof(WeakAppOptions)));
        services.AddHttpClient<IWeakAppApiClient, WeakAppApiClient>(client =>
        {
            var options = configuration.GetSection(nameof(WeakAppOptions)).Get<WeakAppOptions>();

            if (string.IsNullOrEmpty(options?.BaseUrl))
            {
                throw new InvalidOperationException("Base url is not configured for WeakApp api client");
            }

            client.BaseAddress = new Uri(options.BaseUrl);
            client.DefaultRequestHeaders.Add("X-Api-Key", options.ApiKey);
        });

        services.AddHostedService<DataFetcherBackgroundService>();

        return services;
    }
}
