namespace GraphQLGateway.Core.Dto;

using MetersApp.Shared.Enums;

public class MotionReadingDto
{
    public Guid Id { get; set; }

    public LocationType LocationId { get; set; }

    public DateTime Timestamp { get; set; }

    public bool MotionDetected { get; set; }
}
