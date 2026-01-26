namespace Notifications.Api.SignalR;

using Microsoft.AspNetCore.SignalR;
using Notifications.Core.Dto;
using Notifications.Core.Interfaces;

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
