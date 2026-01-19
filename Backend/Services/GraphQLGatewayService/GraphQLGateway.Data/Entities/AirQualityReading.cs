namespace GraphQLGateway.Data.Entities;

public class AirQualityReading : MeterBaseEntity
{
    public Guid Id { get; set; }

    public int Co2 { get; set; }

    public int Pm25 { get; set; }

    public int Humidity { get; set; }
}
