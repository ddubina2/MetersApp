using MetersApp.Shared.Enums;

namespace GraphQLGateway.Core.Dto;

public record EnergyReadingDto(
    Guid Id,
    LocationType LocationId,
    DateTime Timestamp,
    float Energy
);
