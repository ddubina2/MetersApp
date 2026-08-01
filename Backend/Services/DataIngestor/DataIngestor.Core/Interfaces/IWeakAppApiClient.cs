using DataIngestor.Core.Services.WeakAppApiClient.Models;

namespace DataIngestor.Core.Interfaces;

public interface IWeakAppApiClient
{
    Task<List<SensorData>> GetSensorDataAsync(CancellationToken cancellationToken);
}
