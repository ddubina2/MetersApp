using Notifications.Core.Dto;

namespace Notifications.Core.Interfaces;

public interface ISensorBroadcaster
{
    Task BroadcastAsync(SensorDataDto sensorData, CancellationToken cancellationToken);
}
