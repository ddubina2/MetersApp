using MetersApp.Shared.Enums;

namespace GraphQLGateway.Core.Dto;

public class EnergyAggregationDto
{
    public LocationType Location { get; set; }

    public DateTime Day { get; set; }

    public float TotalEnergy { get; set; }
}
