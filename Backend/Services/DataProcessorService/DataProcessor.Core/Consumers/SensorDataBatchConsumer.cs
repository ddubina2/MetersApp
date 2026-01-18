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

    public SensorDataBatchConsumer(DataProcessorDbContext dbContext, ILogger<SensorDataBatchConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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
                        _dbContext.AirQualityReadings,
                        context.CancellationToken);
                    break;

                case SensorType.Energy:
                    await AddReadingAsync(
                        item.Payload,
                        item.LocationType,
                        _dbContext.EnergyReadings,
                        context.CancellationToken);
                    break;

                case SensorType.Motion:
                    await AddReadingAsync(
                        item.Payload,
                        item.LocationType,
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
    }

    private async Task AddReadingAsync<TReading>(
        JsonElement payload,
        LocationType location,
        DbSet<TReading> dbSet,
        CancellationToken ct)
        where TReading : MeterBaseEntity
    {
        var reading = payload.Deserialize<TReading>(JsonSerializerOptions);

        if (reading is null)
        {
            return;
        }

        reading.LocationId = location;
        reading.Timestamp = DateTime.UtcNow;

        await dbSet.AddAsync(reading, ct);
    }
}
