using MetersApp.Shared.Enums;

namespace GraphQLGateway.Core.Dto;

public class MotionReadingDto
{
    public Guid Id { get; set; }
    public LocationType LocationId { get; set; }
    public DateTime Timestamp { get; set; }
    public bool MotionDetected { get; set; }
}
