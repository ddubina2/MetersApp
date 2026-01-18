using System.Text.Json;
using MetersApp.Shared.Enums;

namespace DataIngestor.Core.Services.WeakAppApiClient.Models;

public class SensorData
{
    public SensorType SensorType { get; set; }

    public Location Location { get; set; }

    public JsonElement Payload { get; set; }
}
