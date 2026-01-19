namespace GraphQLGateway.Data.Entities;

public class EnergyReading : MeterBaseEntity
{
    public Guid Id { get; set; }

    public float Energy { get; set; }
}
