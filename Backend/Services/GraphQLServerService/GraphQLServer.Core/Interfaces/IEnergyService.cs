using GraphQLServer.Core.Dto;

namespace GraphQLServer.Core.Interfaces;

public interface IEnergyService
{
    IQueryable<EnergyReadingDto> GetReadings();
}
