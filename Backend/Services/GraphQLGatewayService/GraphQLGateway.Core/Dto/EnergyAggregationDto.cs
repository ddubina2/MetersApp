namespace GraphQLGateway.Core.Dto;

using MetersApp.Shared.Enums;

public class EnergyAggregationDto
{
    public LocationType Location { get; set; }

    public DateTime Day { get; set; }

    public float TotalEnergy { get; set; }
}
