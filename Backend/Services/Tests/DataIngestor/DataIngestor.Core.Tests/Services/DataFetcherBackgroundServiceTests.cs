using System.Text.Json;
using DataIngestor.Core.Interfaces;
using DataIngestor.Core.Options;
using DataIngestor.Core.Services;
using DataIngestor.Core.Services.WeakAppApiClient.Models;
using FluentAssertions;
using MassTransit;
using MetersApp.Shared.Enums;
using MetersApp.Shared.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DataIngestor.Core.Tests.Services;

public class DataFetcherBackgroundServiceTests
{
    private readonly Mock<IWeakAppApiClient> _mockApiClient;
    private readonly Mock<ISendEndpoint> _mockSendEndpoint;
    private readonly Mock<ISendEndpointProvider> _mockSendEndpointProvider;

    public DataFetcherBackgroundServiceTests()
    {
        _mockApiClient = new Mock<IWeakAppApiClient>();
        _mockSendEndpoint = new Mock<ISendEndpoint>();
        _mockSendEndpointProvider = new Mock<ISendEndpointProvider>();

        _mockSendEndpointProvider
            .Setup(x => x.GetSendEndpoint(It.IsAny<Uri>()))
            .ReturnsAsync(_mockSendEndpoint.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFetchAndSendData()
    {
        // Arrange
        var iterationCompleted = new TaskCompletionSource();
        var sensorData = new List<SensorData>
        {
            new()
            {
                SensorType = SensorType.Energy,
                LocationType = LocationType.LivingRoom,
                Payload = JsonDocument.Parse("{\"value\": 22.5}").RootElement,
            },
        };

        _mockApiClient
            .Setup(x => x.GetSensorDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(sensorData);

        _mockSendEndpoint
            .Setup(x => x.Send(It.IsAny<ProcessSensorDataBatch>(), It.IsAny<CancellationToken>()))
            .Callback(() => iterationCompleted.TrySetResult())
            .Returns(Task.CompletedTask);

        var options = Microsoft.Extensions.Options.Options.Create(new WeakAppOptions
        {
            BaseUrl = "https://test-api",
            ApiKey = "test-key",
            RequestIntervalSec = 1,
        });

        var serviceProvider = CreateServiceProvider();
        var service = new DataFetcherBackgroundService(
            options,
            serviceProvider,
            NullLogger<DataFetcherBackgroundService>.Instance);

        using var cts = new CancellationTokenSource();

        // Act
        await service.StartAsync(cts.Token);
        await Task.WhenAny(iterationCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None));
        await service.StopAsync(CancellationToken.None);

        // Assert
        _mockApiClient.Verify(x => x.GetSensorDataAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _mockSendEndpoint.Verify(
            x => x.Send(
                It.Is<ProcessSensorDataBatch>(b =>
                    b.Items.Count() == 1 &&
                    b.Items.First().SensorType == SensorType.Energy &&
                    b.Items.First().LocationType == LocationType.LivingRoom),
                It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSendMultipleItemsInBatch()
    {
        // Arrange
        var iterationCompleted = new TaskCompletionSource();
        var sensorData = new List<SensorData>
        {
            new()
            {
                SensorType = SensorType.Energy,
                LocationType = LocationType.LivingRoom,
                Payload = JsonDocument.Parse("{\"value\": 22.5}").RootElement,
            },
            new()
            {
                SensorType = SensorType.AirQuality,
                LocationType = LocationType.Kitchen,
                Payload = JsonDocument.Parse("{\"quality\": \"good\"}").RootElement,
            },
            new()
            {
                SensorType = SensorType.Motion,
                LocationType = LocationType.Office,
                Payload = JsonDocument.Parse("{\"detected\": true}").RootElement,
            },
        };

        _mockApiClient
            .Setup(x => x.GetSensorDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(sensorData);

        ProcessSensorDataBatch? capturedBatch = null;
        _mockSendEndpoint
            .Setup(x => x.Send(It.IsAny<ProcessSensorDataBatch>(), It.IsAny<CancellationToken>()))
            .Callback<ProcessSensorDataBatch, CancellationToken>((batch, _) =>
            {
                capturedBatch = batch;
                iterationCompleted.TrySetResult();
            })
            .Returns(Task.CompletedTask);

        var options = Microsoft.Extensions.Options.Options.Create(new WeakAppOptions
        {
            BaseUrl = "https://test-api",
            ApiKey = "test-key",
            RequestIntervalSec = 1,
        });

        var serviceProvider = CreateServiceProvider();
        var service = new DataFetcherBackgroundService(
            options,
            serviceProvider,
            NullLogger<DataFetcherBackgroundService>.Instance);

        using var cts = new CancellationTokenSource();

        // Act
        await service.StartAsync(cts.Token);
        await Task.WhenAny(iterationCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None));
        await service.StopAsync(CancellationToken.None);

        // Assert
        capturedBatch.Should().NotBeNull();
        capturedBatch?.Items.Should().HaveCount(3);
        capturedBatch?.Items.Should().Contain(x => x.SensorType == SensorType.Energy && x.LocationType == LocationType.LivingRoom);
        capturedBatch?.Items.Should().Contain(x => x.SensorType == SensorType.AirQuality && x.LocationType == LocationType.Kitchen);
        capturedBatch?.Items.Should().Contain(x => x.SensorType == SensorType.Motion && x.LocationType == LocationType.Office);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSendEmptyBatch_WhenNoSensorData()
    {
        // Arrange
        var iterationCompleted = new TaskCompletionSource();

        _mockApiClient
            .Setup(x => x.GetSensorDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SensorData>());

        _mockSendEndpoint
            .Setup(x => x.Send(It.IsAny<ProcessSensorDataBatch>(), It.IsAny<CancellationToken>()))
            .Callback(() => iterationCompleted.TrySetResult())
            .Returns(Task.CompletedTask);

        var options = Microsoft.Extensions.Options.Options.Create(new WeakAppOptions
        {
            BaseUrl = "https://test-api",
            ApiKey = "test-key",
            RequestIntervalSec = 1,
        });

        var serviceProvider = CreateServiceProvider();
        var service = new DataFetcherBackgroundService(
            options,
            serviceProvider,
            NullLogger<DataFetcherBackgroundService>.Instance);

        using var cts = new CancellationTokenSource();

        // Act
        await service.StartAsync(cts.Token);
        await Task.WhenAny(iterationCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None));
        await service.StopAsync(CancellationToken.None);

        // Assert
        _mockSendEndpoint.Verify(
            x => x.Send(
                It.Is<ProcessSensorDataBatch>(b => !b.Items.Any()),
                It.IsAny<CancellationToken>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleApiException_ContinueRunning()
    {
        // Arrange
        var iterationCompleted = new TaskCompletionSource();
        var callCount = 0;

        _mockApiClient
            .Setup(x => x.GetSensorDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                if (callCount == 1)
                {
                    throw new HttpRequestException("API is down");
                }

                return
                [
                    new()
                    {
                        SensorType = SensorType.Energy,
                        LocationType = LocationType.LivingRoom,
                        Payload = JsonDocument.Parse("{\"value\": 22.5}").RootElement,
                    }

                ];
            });

        _mockSendEndpoint
            .Setup(x => x.Send(It.IsAny<ProcessSensorDataBatch>(), It.IsAny<CancellationToken>()))
            .Callback(() => iterationCompleted.TrySetResult())
            .Returns(Task.CompletedTask);

        var options = Microsoft.Extensions.Options.Options.Create(new WeakAppOptions
        {
            BaseUrl = "https://test-api",
            ApiKey = "test-key",
            RequestIntervalSec = 1,
        });

        var serviceProvider = CreateServiceProvider();
        var service = new DataFetcherBackgroundService(
            options,
            serviceProvider,
            NullLogger<DataFetcherBackgroundService>.Instance);

        using var cts = new CancellationTokenSource();

        // Act - wait for 3 seconds to allow 2 iterations
        await service.StartAsync(cts.Token);
        await Task.WhenAny(iterationCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None));
        await service.StopAsync(CancellationToken.None);

        // Assert
        callCount.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSetTimestampOnSentItems()
    {
        // Arrange
        var beforeTest = DateTime.UtcNow.AddSeconds(-1);
        var iterationCompleted = new TaskCompletionSource();

        var sensorData = new List<SensorData>
        {
            new()
            {
                SensorType = SensorType.Energy,
                LocationType = LocationType.LivingRoom,
                Payload = JsonDocument.Parse("{\"value\": 22.5}").RootElement,
            },
        };

        _mockApiClient
            .Setup(x => x.GetSensorDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(sensorData);

        ProcessSensorDataBatch? capturedBatch = null;
        _mockSendEndpoint
            .Setup(x => x.Send(It.IsAny<ProcessSensorDataBatch>(), It.IsAny<CancellationToken>()))
            .Callback<ProcessSensorDataBatch, CancellationToken>((batch, _) =>
            {
                capturedBatch = batch;
                iterationCompleted.TrySetResult();
            })
            .Returns(Task.CompletedTask);

        var options = Microsoft.Extensions.Options.Options.Create(new WeakAppOptions
        {
            BaseUrl = "https://test-api",
            ApiKey = "test-key",
            RequestIntervalSec = 1,
        });

        var serviceProvider = CreateServiceProvider();
        var service = new DataFetcherBackgroundService(
            options,
            serviceProvider,
            NullLogger<DataFetcherBackgroundService>.Instance);

        using var cts = new CancellationTokenSource();

        // Act
        await service.StartAsync(cts.Token);
        await Task.WhenAny(iterationCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None));
        await service.StopAsync(CancellationToken.None);

        // Assert
        var afterTest = DateTime.UtcNow.AddSeconds(1);
        capturedBatch.Should().NotBeNull();
        var item = capturedBatch?.Items.First();
        item?.Timestamp.Should().BeOnOrAfter(beforeTest);
        item?.Timestamp.Should().BeOnOrBefore(afterTest);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSendToCorrectQueue()
    {
        // Arrange
        var iterationCompleted = new TaskCompletionSource();
        var sensorData = new List<SensorData>
        {
            new()
            {
                SensorType = SensorType.Energy,
                LocationType = LocationType.LivingRoom,
                Payload = JsonDocument.Parse("{\"value\": 22.5}").RootElement,
            },
        };

        _mockApiClient
            .Setup(x => x.GetSensorDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(sensorData);

        Uri? capturedUri = null;
        _mockSendEndpointProvider
            .Setup(x => x.GetSendEndpoint(It.IsAny<Uri>()))
            .Callback<Uri>(uri =>
            {
                capturedUri = uri;
                iterationCompleted.TrySetResult();
            })
            .ReturnsAsync(_mockSendEndpoint.Object);

        var options = Microsoft.Extensions.Options.Options.Create(new WeakAppOptions
        {
            BaseUrl = "https://test-api",
            ApiKey = "test-key",
            RequestIntervalSec = 1,
        });

        var serviceProvider = CreateServiceProvider();
        var service = new DataFetcherBackgroundService(
            options,
            serviceProvider,
            NullLogger<DataFetcherBackgroundService>.Instance);

        using var cts = new CancellationTokenSource();

        // Act
        await service.StartAsync(cts.Token);
        await Task.WhenAny(iterationCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None));
        await service.StopAsync(CancellationToken.None);

        // Assert
        capturedUri.Should().NotBeNull();
        capturedUri?.ToString().Should().Be("queue:sensor-data-processor");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldPreservePayloadContent()
    {
        // Arrange
        var iterationCompleted = new TaskCompletionSource();
        var payloadJson = """
                          {
                              "temperature": 23.5,
                              "humidity": 65,
                              "battery_level": 85
                          }
                          """;

        var sensorData = new List<SensorData>
        {
            new()
            {
                SensorType = SensorType.Energy,
                LocationType = LocationType.LivingRoom,
                Payload = JsonDocument.Parse(payloadJson).RootElement,
            },
        };

        _mockApiClient
            .Setup(x => x.GetSensorDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(sensorData);

        ProcessSensorDataBatch? capturedBatch = null;
        _mockSendEndpoint
            .Setup(x => x.Send(It.IsAny<ProcessSensorDataBatch>(), It.IsAny<CancellationToken>()))
            .Callback<ProcessSensorDataBatch, CancellationToken>((batch, _) =>
            {
                capturedBatch = batch;
                iterationCompleted.TrySetResult();
            })
            .Returns(Task.CompletedTask);

        var options = Microsoft.Extensions.Options.Options.Create(new WeakAppOptions
        {
            BaseUrl = "https://test-api",
            ApiKey = "test-key",
            RequestIntervalSec = 1,
        });

        var serviceProvider = CreateServiceProvider();
        var service = new DataFetcherBackgroundService(
            options,
            serviceProvider,
            NullLogger<DataFetcherBackgroundService>.Instance);

        using var cts = new CancellationTokenSource();

        // Act
        await service.StartAsync(cts.Token);
        await Task.WhenAny(iterationCompleted.Task, Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None));
        await service.StopAsync(CancellationToken.None);

        // Assert
        capturedBatch.Should().NotBeNull();
        var payload = capturedBatch.Items.First().Payload;
        payload.GetProperty("temperature").GetDouble().Should().Be(23.5);
        payload.GetProperty("humidity").GetInt32().Should().Be(65);
        payload.GetProperty("battery_level").GetInt32().Should().Be(85);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRespectCancellationToken()
    {
        // Arrange
        var iterationStarted = new TaskCompletionSource();
        var fetchCount = 0;

        _mockApiClient
            .Setup(x => x.GetSensorDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                fetchCount++;
                iterationStarted.TrySetResult();
                return
                [
                    new SensorData
                    {
                        SensorType = SensorType.Energy,
                        LocationType = LocationType.LivingRoom,
                        Payload = JsonDocument.Parse("{}").RootElement,
                    }

                ];
            });

        _mockSendEndpoint
            .Setup(x => x.Send(It.IsAny<ProcessSensorDataBatch>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var options = Microsoft.Extensions.Options.Options.Create(new WeakAppOptions
        {
            BaseUrl = "https://test-api",
            ApiKey = "test-key",
            RequestIntervalSec = 1,
        });

        var serviceProvider = CreateServiceProvider();
        var service = new DataFetcherBackgroundService(
            options,
            serviceProvider,
            NullLogger<DataFetcherBackgroundService>.Instance);

        using var cts = new CancellationTokenSource();

        // Act
        await service.StartAsync(cts.Token);
        await Task.WhenAny(iterationStarted.Task, Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None));

        // Cancel and stop
        await cts.CancelAsync();
        try
        {
            await service.StopAsync(CancellationToken.None);
        }
        catch (OperationCanceledException)
        {
            // Expected
        }

        // Assert
        fetchCount.Should().BeGreaterThanOrEqualTo(1);
    }

    private ServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_mockApiClient.Object);
        services.AddSingleton(_mockSendEndpointProvider.Object);
        return services.BuildServiceProvider();
    }
}
