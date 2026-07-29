using DataCleaner.Core.Interfaces;
using DataCleaner.Core.Options;
using DataCleaner.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataCleaner.Core;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureCoreServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DataCleanerOptions>(configuration.GetSection(nameof(DataCleanerOptions)));
        services.AddScoped<ISensorDataCleanupService, SensorDataCleanupService>();
        services.AddHostedService<SensorDataCleanupBackgroundService>();

        return services;
    }
}
