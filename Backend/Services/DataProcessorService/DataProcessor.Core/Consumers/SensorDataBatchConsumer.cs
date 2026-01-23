using System.Text.Json;
using DataProcessor.Data;
using DataProcessor.Data.Entities;
using MassTransit;
using MetersApp.Shared.Enums;
using MetersApp.Shared.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataProcessor.Core.Consumers;

public class SensorDataBatchConsumer : IConsumer<ProcessSensorDataBatch>
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    private readonly DataProcessorDbContext _dbContext;
    private readonly ILogger<SensorDataBatchConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public SensorDataBatchConsumer(
        DataProcessorDbContext dbContext,
        ILogger<SensorDataBatchConsumer> logger,
        IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint
    }

    public async Task Consume(ConsumeContext<ProcessSensorDataBatch> context)
    {
        foreach (var item in context.Message.Items)
        {
            switch (item.SensorType)
            {
                case SensorType.AirQuality:
                    await AddReadingAsync(
                        item.Payload,
                        item.LocationType,
                        item.Timestamp,
                        _dbContext.AirQualityReadings,
                        context.CancellationToken);
                    break;

                case SensorType.Energy:
                    await AddReadingAsync(
                        item.Payload,
                        item.LocationType,
                        item.Timestamp,
                        _dbContext.EnergyReadings,
                        context.CancellationToken);
                    break;

                case SensorType.Motion:
                    await AddReadingAsync(
                        item.Payload,
                        item.LocationType,
                        item.Timestamp,
                        _dbContext.MotionReadings,
                        context.CancellationToken);
                    break;

                case SensorType.Unknown:
                default:
                    _logger.LogWarning("Unknown sensor type: {SensorType}", item.SensorType);
                    break;
            }
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        await _publishEndpoint.Publish(new NewSensorDataEvent
        {
            Items = context.Message.Items,
        });
    }

    private static async Task AddReadingAsync<TReading>(
        JsonElement payload,
        LocationType location,
        DateTime timestamp,
        DbSet<TReading> dbSet,
        CancellationToken ct) where TReading : MeterBaseEntity
    {
        var reading = payload.Deserialize<TReading>(JsonSerializerOptions);

        if (reading is null)
        {
            return;
        }

        reading.LocationId = location;
        reading.Timestamp = timestamp;

        await dbSet.AddAsync(reading, ct);
    }
}
