namespace DataCleaner.Api.Models;

public record CleanupSensorDataRequest
{
    public DateTime OlderThan { get; set; }
}
