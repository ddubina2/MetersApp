using MetersApp.Shared.Enums;

namespace GraphQLGateway.Core.Dto;

public class EnergyReadingDto
{
    public Guid Id { get; set; }

    public LocationType LocationId { get; set; }

    public DateTime Timestamp { get; set; }

    public float Energy { get; set; }
}
