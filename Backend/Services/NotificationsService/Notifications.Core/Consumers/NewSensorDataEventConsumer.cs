namespace Notifications.Core.Consumers;

using MassTransit;
using MetersApp.Shared.Messages;
using Microsoft.Extensions.Logging;
using Notifications.Core.Dto;
using Notifications.Core.Interfaces;

public class NewSensorDataEventConsumer : IConsumer<NewSensorDataEvent>
{
    private readonly ILogger<NewSensorDataEventConsumer> _logger;
    private readonly ISensorBroadcaster _sensorBroadcaster;

    public NewSensorDataEventConsumer(ILogger<NewSensorDataEventConsumer> logger, ISensorBroadcaster sensorBroadcaster)
    {
        _logger = logger;
        _sensorBroadcaster = sensorBroadcaster;
    }

    public async Task Consume(ConsumeContext<NewSensorDataEvent> context)
    {
        try
        {
            await _sensorBroadcaster.BroadcastAsync(
                new SensorDataDto
                {
                    Items = context.Message.Items.Select(i => new SensorDataItemDto
                    {
                        LocationType = i.LocationType,
                        SensorType = i.SensorType,
                        Timestamp = i.Timestamp,
                        Payload = i.Payload,
                    }),
                },
                context.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred consuming the message");
        }
    }
}
