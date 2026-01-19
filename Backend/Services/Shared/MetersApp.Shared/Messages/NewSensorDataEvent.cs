namespace MetersApp.Shared.Messages;

public class NewSensorDataEvent
{
    public IEnumerable<SensorDataItem> Items { get; init; } = [];
}
