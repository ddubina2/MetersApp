using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;
using GraphQLGateway.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLGateway.Core.Services;

public class MotionService : IMotionService
{
    private readonly AppDbContext _dbContext;

    public MotionService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<MotionReadingDto> GetReadings()
    {
        return _dbContext.MotionReadings
            .AsNoTracking()
            .Select(x => new MotionReadingDto(
                x.Id,
                x.LocationId,
                x.Timestamp,
                x.MotionDetected
            ));
    }
}
