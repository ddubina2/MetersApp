using MetersApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProcessor.Data.Entities.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> location)
    {
        location.HasKey(l => l.Id);

        location.HasData(
                new Location { Id = LocationType.Office },
                new Location { Id = LocationType.Bedroom },
                new Location { Id = LocationType.Kitchen },
                new Location { Id = LocationType.LivingRoom },
                new Location { Id = LocationType.Corridor },
                new Location { Id = LocationType.Garage }
            );

        location
            .HasMany<AirQualityReading>(l => l.AirQualityReadings)
            .WithOne(l => l.Location)
            .HasForeignKey(l => l.LocationId);

        location
            .HasMany<EnergyReading>(l => l.EnergyReadings)
            .WithOne(l => l.Location)
            .HasForeignKey(l => l.LocationId);

        location
            .HasMany<MotionReading>(l => l.MotionReadings)
            .WithOne(l => l.Location)
            .HasForeignKey(l => l.LocationId);
    }
}
