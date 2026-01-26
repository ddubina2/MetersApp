namespace Notifications.Core.Dto;

using System.Text.Json;
using MetersApp.Shared.Enums;

public class SensorDataDto
{
    public IEnumerable<SensorDataItemDto> Items { get; init; } = [];
}

public class SensorDataItemDto
{
    public SensorType SensorType { get; set; }

    public LocationType LocationType { get; set; }

    public DateTime Timestamp { get; set; }

    public JsonElement Payload { get; set; }
}
