using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataCleaner.Data.Entities.Configurations;

public class EnergyReadingConfiguration : IEntityTypeConfiguration<EnergyReading>
{
    public void Configure(EntityTypeBuilder<EnergyReading> energyReading)
    {
        energyReading.HasKey(er => er.Id);

        energyReading
            .HasOne<Location>(er => er.Location)
            .WithMany(l => l.EnergyReadings)
            .HasForeignKey(er => er.LocationId);
    }
}
