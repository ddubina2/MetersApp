using MetersApp.Shared.Enums;

namespace DataCleaner.Data.Entities;

public class MeterBaseEntity
{
    public LocationType LocationId { get; set; }

    public Location? Location { get; set; }

    public DateTime Timestamp { get; set; }
}
