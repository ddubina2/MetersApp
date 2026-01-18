using System.Text.Json;
using MetersApp.Shared.Enums;

namespace MetersApp.Shared.Messages;

public class ProcessSensorDataBatch
{
    public IEnumerable<SensorDataItem> Items { get; init; } = [];
}

public class SensorDataItem
{
    public SensorType SensorType { get; set; }

    public LocationType LocationType { get; set; }

    public JsonElement Payload { get; set; }
}
