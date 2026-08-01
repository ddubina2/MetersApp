using DataCleaner.Core.Options;
using DataCleaner.Core.Services.NextCleanupDateStorage;
using DataCleaner.Core.Services.SensorDataCleanup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataCleaner.Core.BackgroundJobs;

public class SensorDataCleanupBackgroundJob : BackgroundService
{
    private readonly DataCleanerOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SensorDataCleanupBackgroundJob> _logger;

    public SensorDataCleanupBackgroundJob(
        IOptions<DataCleanerOptions> options,
        IServiceProvider serviceProvider,
        ILogger<SensorDataCleanupBackgroundJob> logger)
    {
        _options = options.Value;
        _serviceProvider = serviceProvider;
        _logger = logger;

        if (_options.RunIntervalMinutes <= 0)
        {
            throw new ArgumentException(
                $"{nameof(DataCleanerOptions.RunIntervalMinutes)} must be greater than 0.", nameof(options));
        }

        if (_options.RetentionDays <= 0)
        {
            throw new ArgumentException(
                $"{nameof(DataCleanerOptions.RetentionDays)} must be greater than 0.", nameof(options));
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var cleanupService = scope.ServiceProvider.GetRequiredService<ISensorDataCleanupService>();
        var nextDateStorage = scope.ServiceProvider.GetRequiredService<INextCleanupDateStorage>();
        var minutes = TimeSpan.FromMinutes(_options.RunIntervalMinutes);

        await nextDateStorage.Set(DateTime.UtcNow.Add(minutes));

        using PeriodicTimer timer = new(minutes);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                var olderThan = DateTime.UtcNow.AddDays(-_options.RetentionDays);
                _logger.LogInformation("Deleting sensor data older than {OlderThan}", olderThan);

                var result = await cleanupService.DeleteOldSensorDataAsync(olderThan, stoppingToken);
                await nextDateStorage.Set(DateTime.UtcNow.Add(minutes));

                _logger.LogInformation("Sensor data cleanup completed successfully");
                _logger.LogInformation("Air quality records deleted: {AirQualityDeletedCount}", result.AirQualityDeletedCount);
                _logger.LogInformation("Energy records deleted: {EnergyDeletedCount}", result.EnergyDeletedCount);
                _logger.LogInformation("Motion records deleted: {MotionDeletedCount}", result.MotionDeletedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cleaning up old sensor data.");
            }
        }
    }
}
