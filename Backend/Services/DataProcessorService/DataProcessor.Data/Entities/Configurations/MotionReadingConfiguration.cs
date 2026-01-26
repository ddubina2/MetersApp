namespace DataProcessor.Data.Entities.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MotionReadingConfiguration : IEntityTypeConfiguration<MotionReading>
{
    public void Configure(EntityTypeBuilder<MotionReading> motionReading)
    {
        motionReading.HasKey(mr => mr.Id);

        motionReading
            .HasOne<Location>(mr => mr.Location)
            .WithMany(l => l.MotionReadings)
            .HasForeignKey(mr => mr.LocationId);
    }
}
