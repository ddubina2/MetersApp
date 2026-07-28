using GraphQLGateway.Core.Dto;

namespace GraphQLGateway.Core.Interfaces;

public interface IMotionService
{
    IQueryable<MotionReadingDto> GetReadings();
}
