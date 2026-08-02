using GraphQLServer.Core.Dto;
using GraphQLServer.Core.Interfaces;
using GraphQLServer.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Core.Services;

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
