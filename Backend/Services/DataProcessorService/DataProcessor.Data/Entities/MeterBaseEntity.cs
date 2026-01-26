namespace DataProcessor.Data.Entities;

using MetersApp.Shared.Enums;

public class MeterBaseEntity
{
    public LocationType LocationId { get; set; }

    public Location? Location { get; set; }

    public DateTime Timestamp { get; set; }
}
