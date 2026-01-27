namespace GraphQLGateway.Core.Dto;

using MetersApp.Shared.Enums;

public class AirQualityReadingDto
{
    public Guid Id { get; set; }

    public LocationType LocationId { get; set; }

    public DateTime Timestamp { get; set; }

    public int Co2 { get; set; }

    public int Pm25 { get; set; }

    public int Humidity { get; set; }
}
