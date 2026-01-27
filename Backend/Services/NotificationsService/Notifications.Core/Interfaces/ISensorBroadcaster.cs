namespace Notifications.Core.Interfaces;

using Notifications.Core.Dto;

public interface ISensorBroadcaster
{
    Task BroadcastAsync(SensorDataDto sensorData, CancellationToken cancellationToken);
}
