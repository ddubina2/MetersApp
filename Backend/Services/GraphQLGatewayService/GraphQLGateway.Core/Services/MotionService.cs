namespace GraphQLGateway.Core.Services;

using GraphQLGateway.Core.Dto;
using GraphQLGateway.Core.Interfaces;
using GraphQLGateway.Data;
using Microsoft.EntityFrameworkCore;

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
            .Select(x => new MotionReadingDto
            {
                Id = x.Id,
                LocationId = x.LocationId,
                Timestamp = x.Timestamp,
                MotionDetected = x.MotionDetected,
            });
    }
}
