namespace GraphQLGateway.Data.Entities;

public class MotionReading : MeterBaseEntity
{
    public Guid Id { get; set; }

    public bool MotionDetected { get; set; }
}
