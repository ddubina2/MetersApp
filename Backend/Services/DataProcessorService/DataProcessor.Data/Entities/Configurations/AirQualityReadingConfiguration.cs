using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProcessor.Data.Entities.Configurations;

public class AirQualityReadingConfiguration : IEntityTypeConfiguration<AirQualityReading>
{
    public void Configure(EntityTypeBuilder<AirQualityReading> aqReading)
    {
        aqReading.HasKey(aq => aq.Id);

        aqReading
            .HasOne<Location>(aq => aq.Location)
            .WithMany(l => l.AirQualityReadings)
            .HasForeignKey(aq => aq.LocationId);
    }
}
