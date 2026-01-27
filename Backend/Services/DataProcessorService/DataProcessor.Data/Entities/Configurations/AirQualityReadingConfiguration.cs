namespace DataProcessor.Data.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
