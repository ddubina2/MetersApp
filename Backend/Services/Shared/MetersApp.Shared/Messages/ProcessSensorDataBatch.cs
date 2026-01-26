namespace MetersApp.Shared.Messages;

using System.Text.Json;
using MetersApp.Shared.Enums;

public class ProcessSensorDataBatch
{
    public IEnumerable<SensorDataItem> Items { get; init; } = [];
}

public class SensorDataItem
{
    public SensorType SensorType { get; set; }

    public LocationType LocationType { get; set; }

    public DateTime Timestamp { get; set; }

    public JsonElement Payload { get; set; }
}
