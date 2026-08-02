using MetersApp.Shared.Enums;

namespace DataProcessor.Data.Entities;

public class Location
{
    public LocationType Id { get; set; }

    public ICollection<AirQualityReading> AirQualityReadings { get; set; } = [];

    public ICollection<MotionReading> MotionReadings { get; set; } = [];

    public ICollection<EnergyReading> EnergyReadings { get; set; } = [];
}
