using DataCleaner.Core.BackgroundJobs;
using DataCleaner.Core.Options;
using DataCleaner.Core.Services.NextCleanupDateStorage;
using DataCleaner.Core.Services.SensorDataCleanup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataCleaner.Core;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureCoreServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.Configure<DataCleanerOptions>(configuration.GetSection(nameof(DataCleanerOptions)));
        services.AddHostedService<SensorDataCleanupBackgroundJob>();

        services.AddScoped<ISensorDataCleanupService, SensorDataCleanupService>();
        services.AddScoped<ICleanupStatusStorage, CacheCleanupStatusStorage>();

        return services;
    }
}
