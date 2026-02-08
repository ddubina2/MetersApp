namespace Notifications.Core.Tests.SignalR;

using System.Text.Json;
using FluentAssertions;
using MetersApp.Shared.Enums;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Notifications.Api.SignalR;
using Notifications.Core.Dto;

public class SignalRSensorBroadcasterTests
{
    private readonly Mock<IHubContext<SensorHub>> _mockHubContext;
    private readonly Mock<IClientProxy> _mockClientsProxy;
    private readonly SignalRSensorBroadcaster _broadcaster;

    public SignalRSensorBroadcasterTests()
    {
        _mockHubContext = new Mock<IHubContext<SensorHub>>();
        _mockClientsProxy = new Mock<IClientProxy>();

        _mockHubContext
            .Setup(x => x.Clients.All)
            .Returns(_mockClientsProxy.Object);

        _broadcaster = new SignalRSensorBroadcaster(_mockHubContext.Object);
    }

    [Fact]
    public async Task BroadcastAsync_ShouldCallClientsAllSendAsync()
    {
        // Arrange
        var sensorData = new SensorDataDto
        {
            Items = new List<SensorDataItemDto>(),
        };

        _mockClientsProxy
            .Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _broadcaster.BroadcastAsync(sensorData, CancellationToken.None);

        // Assert
        _mockClientsProxy.Verify(
            x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task BroadcastAsync_ShouldSendCorrectMethodName()
    {
        // Arrange
        var sensorData = new SensorDataDto
        {
            Items = new List<SensorDataItemDto>(),
        };

        string? capturedMethodName = null;
        _mockClientsProxy
            .Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, object?[], CancellationToken>((methodName, _, _) =>
            {
                capturedMethodName = methodName;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _broadcaster.BroadcastAsync(sensorData, CancellationToken.None);

        // Assert
        capturedMethodName.Should().Be("ReceiveSensorData");
    }

    [Fact]
    public async Task BroadcastAsync_ShouldPassSensorDataDto()
    {
        // Arrange
        var timestamp = DateTime.UtcNow;
        var payload = JsonDocument.Parse("""{"value": 22.5}""").RootElement.Clone();

        var sensorData = new SensorDataDto
        {
            Items = new List<SensorDataItemDto>
            {
                new()
                {
                    SensorType = SensorType.Energy,
                    LocationType = LocationType.LivingRoom,
                    Timestamp = timestamp,
                    Payload = payload,
                },
            },
        };

        object?[]? capturedArgs = null;
        _mockClientsProxy
            .Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, object?[], CancellationToken>((_, args, _) =>
            {
                capturedArgs = args;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _broadcaster.BroadcastAsync(sensorData, CancellationToken.None);

        // Assert
        capturedArgs.Should().NotBeNull();
        capturedArgs.Should().HaveCount(1);
        capturedArgs![0].Should().Be(sensorData);
    }

    [Fact]
    public async Task BroadcastAsync_ShouldPassCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        var sensorData = new SensorDataDto
        {
            Items = new List<SensorDataItemDto>(),
        };

        CancellationToken? capturedToken = null;
        _mockClientsProxy
            .Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, object?[], CancellationToken>((_, _, ct) =>
            {
                capturedToken = ct;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _broadcaster.BroadcastAsync(sensorData, token);

        // Assert
        capturedToken.Should().Be(token);
    }

    [Fact]
    public async Task BroadcastAsync_WithMultipleItems_ShouldPassCorrectData()
    {
        // Arrange
        var timestamp = DateTime.UtcNow;

        var sensorData = new SensorDataDto
        {
            Items = new List<SensorDataItemDto>
            {
                new()
                {
                    SensorType = SensorType.Energy,
                    LocationType = LocationType.LivingRoom,
                    Timestamp = timestamp,
                    Payload = JsonDocument.Parse("""{"energy": 100}""").RootElement.Clone(),
                },
                new()
                {
                    SensorType = SensorType.AirQuality,
                    LocationType = LocationType.Kitchen,
                    Timestamp = timestamp.AddMinutes(1),
                    Payload = JsonDocument.Parse("""{"co2": 450}""").RootElement.Clone(),
                },
            },
        };

        SensorDataDto? capturedDto = null;
        _mockClientsProxy
            .Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, object?[], CancellationToken>((_, args, _) =>
            {
                capturedDto = args[0] as SensorDataDto;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _broadcaster.BroadcastAsync(sensorData, CancellationToken.None);

        // Assert
        capturedDto.Should().NotBeNull();
        capturedDto!.Items.Should().HaveCount(2);

        var items = capturedDto.Items.ToList();
        items[0].SensorType.Should().Be(SensorType.Energy);
        items[0].LocationType.Should().Be(LocationType.LivingRoom);

        items[1].SensorType.Should().Be(SensorType.AirQuality);
        items[1].LocationType.Should().Be(LocationType.Kitchen);
    }

    [Fact]
    public async Task BroadcastAsync_WithEmptyItems_ShouldPassEmptyDto()
    {
        // Arrange
        var sensorData = new SensorDataDto
        {
            Items = new List<SensorDataItemDto>(),
        };

        SensorDataDto? capturedDto = null;
        _mockClientsProxy
            .Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, object?[], CancellationToken>((_, args, _) =>
            {
                capturedDto = args[0] as SensorDataDto;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _broadcaster.BroadcastAsync(sensorData, CancellationToken.None);

        // Assert
        capturedDto.Should().NotBeNull();
        capturedDto!.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task BroadcastAsync_ShouldThrow_WhenHubFails()
    {
        // Arrange
        var sensorData = new SensorDataDto
        {
            Items = new List<SensorDataItemDto>(),
        };

        _mockClientsProxy
            .Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Hub connection failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _broadcaster.BroadcastAsync(sensorData, CancellationToken.None));
    }

    [Fact]
    public async Task BroadcastAsync_MultipleCalls_ShouldSendEachMessage()
    {
        // Arrange
        var sensorData1 = new SensorDataDto
        {
            Items = new List<SensorDataItemDto>
            {
                new()
                {
                    SensorType = SensorType.Energy,
                    LocationType = LocationType.LivingRoom,
                    Timestamp = DateTime.UtcNow,
                    Payload = JsonDocument.Parse("""{"value": 100}""").RootElement.Clone(),
                },
            },
        };

        var sensorData2 = new SensorDataDto
        {
            Items = new List<SensorDataItemDto>
            {
                new()
                {
                    SensorType = SensorType.Motion,
                    LocationType = LocationType.Kitchen,
                    Timestamp = DateTime.UtcNow,
                    Payload = JsonDocument.Parse("""{"detected": true}""").RootElement.Clone(),
                },
            },
        };

        _mockClientsProxy
            .Setup(x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _broadcaster.BroadcastAsync(sensorData1, CancellationToken.None);
        await _broadcaster.BroadcastAsync(sensorData2, CancellationToken.None);

        // Assert
        _mockClientsProxy.Verify(
            x => x.SendCoreAsync(
                It.IsAny<string>(),
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }
}
