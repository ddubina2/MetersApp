using Microsoft.AspNetCore.SignalR;
using Notifications.Core.Dto;
using Notifications.Core.Interfaces;

namespace Notifications.Api.SignalR;

public class SignalRSensorBroadcaster : ISensorBroadcaster
{
    private readonly IHubContext<SensorHub> _hubContext;

    public SignalRSensorBroadcaster(IHubContext<SensorHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task BroadcastAsync(SensorDataDto sensorData, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.SendAsync("ReceiveSensorData", sensorData, cancellationToken);
    }
}
