using System.Text.Json;
using MetersApp.Shared.Enums;

namespace Notifications.Core.Dto;

public class SensorDataDto
{
    public IEnumerable<SensorDataItemDto> Items { get; init; } = [];
}

public class SensorDataItemDto
{
    public SensorType SensorType { get; set; }

    public LocationType LocationType { get; set; }

    public JsonElement Payload { get; set; }
}
