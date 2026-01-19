using MetersApp.Shared.Enums;

namespace GraphQLGateway.Core.Dto;

public record EnergyAggregationDto(
    LocationType Location,
    DateTime Day,
    float TotalEnergy
);
