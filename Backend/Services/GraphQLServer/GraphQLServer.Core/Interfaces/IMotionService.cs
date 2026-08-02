using GraphQLServer.Core.Dto;

namespace GraphQLServer.Core.Interfaces;

public interface IMotionService
{
    IQueryable<MotionReadingDto> GetReadings();
}
