namespace DataIngestor.Core.Interfaces;

using DataIngestor.Core.Services.WeakAppApiClient.Models;

public interface IWeakAppApiClient
{
    Task<List<SensorData>> GetSensorDataAsync(CancellationToken cancellationToken);
}
