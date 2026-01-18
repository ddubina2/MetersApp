using System.Text.Json;
using DataIngestor.Core.Interfaces;
using DataIngestor.Core.Options;
using DataIngestor.Core.Services.WeakAppApiClient.Models;
using MetersApp.Shared.Enums;
using MetersApp.Shared.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace DataIngestor.Core.Services.WeakAppApiClient;

public class WeakAppApiClient : IWeakAppApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeakAppApiClient> _logger;
    private readonly AsyncRetryPolicy<List<SensorData>> _retryPolicy;
    private readonly WeakAppOptions _weakAppOptions;

    public WeakAppApiClient(
        HttpClient httpClient,
        ILogger<WeakAppApiClient> logger,
        IOptions<WeakAppOptions> weakAppOptions)
    {
        _httpClient = httpClient;
        _logger = logger;
        _weakAppOptions = weakAppOptions.Value;

        _retryPolicy = Policy<List<SensorData>>
            .Handle<HttpRequestException>()
            .Or<JsonException>()
            .OrResult(result => result.Count == 0)
            .WaitAndRetryAsync(
                retryCount: 1,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // 2s, 4s, 8s
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    var reason = outcome.Exception?.Message ?? "empty/corrupted data";
                    _logger.LogWarning("Retry {RetryAttempt} after {Delay}s due to: {Reason}",
                        retryAttempt, timespan.TotalSeconds, reason);
                });
    }

    public async Task<List<SensorData>> GetSensorDataAsync(CancellationToken cancellationToken)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var sensorDataList = new List<SensorData>();

            var response = await _httpClient.GetAsync("/meters", cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogWarning("API returned empty response.");
                return sensorDataList;
            }

            using var jsonDoc = JsonDocument.Parse(json);
            var root = jsonDoc.RootElement;

            if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("error", out var errorProp))
            {
                _logger.LogWarning("API returned error: {Error}", errorProp.GetString());
                return sensorDataList;
            }

            if (root.ValueKind != JsonValueKind.Array)
            {
                _logger.LogWarning("Unexpected API response format.");
                return sensorDataList;
            }

            foreach (var element in root.EnumerateArray())
            {
                if (!element.TryGetProperty("type", out var typeProp) ||
                    !element.TryGetProperty("name", out var nameProp) ||
                    !element.TryGetProperty("payload", out var payloadProp))
                {
                    _logger.LogWarning("Skipping malformed sensor object.");
                    continue;
                }

                var typeString = typeProp.GetString()?.SnakeToPascalCase();
                if (!Enum.TryParse<SensorType>(typeString, true, out var sensorType)
                    || sensorType == SensorType.Unknown)
                {
                    _logger.LogWarning("Unknown SensorType: {Type}, skipping...", typeString);
                    continue;
                }

                var nameString = nameProp.GetString()?.Replace(" ", "",
                    StringComparison.InvariantCultureIgnoreCase);
                if (!Enum.TryParse<LocationType>(nameString, true, out var location) ||
                    location == LocationType.Unknown)
                {
                    _logger.LogWarning("Unknown Location: {Location}, skipping...", nameString);
                    continue;
                }

                sensorDataList.Add(new SensorData
                {
                    SensorType = sensorType,
                    LocationType = location,
                    Payload = payloadProp.Clone()
                });
            }

            return sensorDataList;
        });
    }
}
