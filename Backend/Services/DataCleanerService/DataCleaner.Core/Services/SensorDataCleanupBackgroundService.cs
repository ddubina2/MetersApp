using DataCleaner.Core.Interfaces;
using DataCleaner.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataCleaner.Core.Services;

public class SensorDataCleanupBackgroundService : BackgroundService
{
    private readonly DataCleanerOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SensorDataCleanupBackgroundService> _logger;

    public SensorDataCleanupBackgroundService(
        IOptions<DataCleanerOptions> options,
        IServiceProvider serviceProvider,
        ILogger<SensorDataCleanupBackgroundService> logger)
    {
        _options = options.Value;
        _serviceProvider = serviceProvider;
        _logger = logger;

        if (_options.RunIntervalHours <= 0)
        {
            throw new ArgumentException(
                $"{nameof(DataCleanerOptions.RunIntervalHours)} must be greater than 0.",
                nameof(options));
        }

        if (_options.RetentionDays <= 0)
        {
            throw new ArgumentException(
                $"{nameof(DataCleanerOptions.RetentionDays)} must be greater than 0.",
                nameof(options));
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var cleanupService = scope.ServiceProvider.GetRequiredService<ISensorDataCleanupService>();
        using PeriodicTimer timer = new(TimeSpan.FromHours(_options.RunIntervalHours));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                var olderThan = DateTime.UtcNow.AddDays(-_options.RetentionDays);
                _logger.LogInformation(
                    "Deleting sensor data older than {OlderThan}", olderThan);

                await cleanupService.DeleteOldSensorDataAsync(olderThan, stoppingToken);

                _logger.LogInformation("Sensor data cleanup completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cleaning up old sensor data.");
            }
        }
    }
}
