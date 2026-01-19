using MetersApp.Shared.Enums;

namespace GraphQLGateway.Core.Dto;

public record MotionReadingDto(
    Guid Id,
    LocationType LocationId,
    DateTime Timestamp,
    bool MotionDetected
);
