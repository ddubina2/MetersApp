namespace GraphQLGateway.Core.Interfaces;

using GraphQLGateway.Core.Dto;

public interface IMotionService
{
    IQueryable<MotionReadingDto> GetReadings();
}
