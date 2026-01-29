namespace DataIngestor.Core.Services;

using DataIngestor.Core.Interfaces;
using DataIngestor.Core.Options;
using MassTransit;
using MetersApp.Shared.Constants;
using MetersApp.Shared.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class DataFetcherBackgroundService : BackgroundService
{
    private readonly WeakAppOptions _weakAppOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataFetcherBackgroundService> _logger;

    public DataFetcherBackgroundService(
        IOptions<WeakAppOptions> weakAppOptions,
        IServiceProvider serviceProvider,
        ILogger<DataFetcherBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _weakAppOptions = weakAppOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(_weakAppOptions.RequestIntervalSec));
        var weakAppClient = scope.ServiceProvider.GetRequiredService<IWeakAppApiClient>();
        var endpoint = await scope.ServiceProvider
            .GetRequiredService<ISendEndpointProvider>()
            .GetSendEndpoint(new Uri($"queue:{QueueNames.ProcessSensorDataQueue}"));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                var sensorData = await weakAppClient.GetSensorDataAsync(stoppingToken);

                await endpoint.Send(
                    new ProcessSensorDataBatch
                    {
                        Items = sensorData.Select(x => new SensorDataItem
                        {
                            SensorType = x.SensorType,
                            LocationType = x.LocationType,
                            Payload = x.Payload,
                            Timestamp = DateTime.UtcNow,
                        }),
                    },
                    stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching sensor data in background job.");
            }
        }
    }
}
