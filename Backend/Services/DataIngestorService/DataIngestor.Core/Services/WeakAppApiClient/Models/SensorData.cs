namespace DataIngestor.Core.Services.WeakAppApiClient.Models;

using System.Text.Json;
using MetersApp.Shared.Enums;

public class SensorData
{
    public SensorType SensorType { get; set; }

    public LocationType LocationType { get; set; }

    public JsonElement Payload { get; set; }
}
