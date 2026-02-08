namespace DataIngestor.Core.Tests.Services;

using System.Net;
using System.Text;
using DataIngestor.Core.Services.WeakAppApiClient;
using FluentAssertions;
using MetersApp.Shared.Enums;
using Microsoft.Extensions.Logging.Abstractions;

public class WeakAppApiClientTests
{
    [Fact]
    public async Task GetSensorDataAsync_ShouldReturnEmptyList_WhenResponseIsEmpty()
    {
        // Arrange
        var client = CreateClient(string.Empty);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldReturnEmptyList_WhenApiReturnsErrorObject()
    {
        // Arrange
        var json = """
                   {
                       "error": "Something went wrong"
                   }
                   """;

        var client = CreateClient(json);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldSkipMalformedItems()
    {
        // Arrange
        var json = """
                   [
                       { "type": "temperature", "name": "Kitchen" },
                       { "type": "humidity", "payload": {} }
                   ]
                   """;

        var client = CreateClient(json);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldReturnValidSensorData()
    {
        // Arrange
        var json = """
                   [
                       {
                           "type": "energy",
                           "name": "Living Room",
                           "payload": { "value": 22.5 }
                       }
                   ]
                   """;

        var client = CreateClient(json);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);

        var item = result[0];
        item.SensorType.Should().Be(SensorType.Energy);
        item.LocationType.Should().Be(LocationType.LivingRoom);
        item.Payload.GetProperty("value").GetDouble().Should().Be(22.5);
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldRetry_WhenFirstResponseIsEmpty()
    {
        // Arrange
        var callCount = 0;

        var handler = new FakeHttpMessageHandler(_ =>
        {
            callCount++;

            if (callCount == 1)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[]", Encoding.UTF8, "application/json"),
                };
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """
                    [
                        {
                            "type": "energy",
                            "name": "Kitchen",
                            "payload": { "value": 20 }
                        }
                    ]
                    """,
                    Encoding.UTF8,
                    "application/json"),
            };
        });

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://fake-api"),
        };

        var client = new WeakAppApiClient(
            httpClient,
            NullLogger<WeakAppApiClient>.Instance);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        callCount.Should().Be(2);
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldReturnMultipleItems()
    {
        // Arrange
        var json = """
                   [
                       {
                           "type": "energy",
                           "name": "Living Room",
                           "payload": { "value": 22.5 }
                       },
                       {
                           "type": "air_quality",
                           "name": "Kitchen",
                           "payload": { "quality": "good" }
                       },
                       {
                           "type": "motion",
                           "name": "Office",
                           "payload": { "detected": true }
                       }
                   ]
                   """;

        var client = CreateClient(json);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result[0].SensorType.Should().Be(SensorType.Energy);
        result[0].LocationType.Should().Be(LocationType.LivingRoom);
        result[1].SensorType.Should().Be(SensorType.AirQuality);
        result[1].LocationType.Should().Be(LocationType.Kitchen);
        result[2].SensorType.Should().Be(SensorType.Motion);
        result[2].LocationType.Should().Be(LocationType.Office);
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldSkipUnknownSensorType()
    {
        // Arrange
        var json = """
                   [
                       {
                           "type": "unknown_type",
                           "name": "Living Room",
                           "payload": { "value": 22.5 }
                       },
                       {
                           "type": "energy",
                           "name": "Kitchen",
                           "payload": { "value": 20 }
                       }
                   ]
                   """;

        var client = CreateClient(json);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].SensorType.Should().Be(SensorType.Energy);
        result[0].LocationType.Should().Be(LocationType.Kitchen);
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldSkipUnknownLocation()
    {
        // Arrange
        var json = """
                   [
                       {
                           "type": "energy",
                           "name": "Unknown Room",
                           "payload": { "value": 22.5 }
                       },
                       {
                           "type": "energy",
                           "name": "Office",
                           "payload": { "value": 20 }
                       }
                   ]
                   """;

        var client = CreateClient(json);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].LocationType.Should().Be(LocationType.Office);
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldHandleHttpErrorStatusCode()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(_ =>
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Server error", Encoding.UTF8, "text/plain"),
            };
        });

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://fake-api"),
        };

        var client = new WeakAppApiClient(
            httpClient,
            NullLogger<WeakAppApiClient>.Instance);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => client.GetSensorDataAsync(CancellationToken.None));
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldRetryOnHttpRequestException()
    {
        // Arrange
        var callCount = 0;

        var handler = new FakeHttpMessageHandler(_ =>
        {
            callCount++;

            if (callCount == 1)
            {
                throw new HttpRequestException("Network error");
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """
                    [
                        {
                            "type": "energy",
                            "name": "Kitchen",
                            "payload": { "value": 20 }
                        }
                    ]
                    """,
                    Encoding.UTF8,
                    "application/json"),
            };
        });

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://fake-api"),
        };

        var client = new WeakAppApiClient(
            httpClient,
            NullLogger<WeakAppApiClient>.Instance);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        callCount.Should().Be(2);
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldRetryOnJsonException()
    {
        // Arrange
        var callCount = 0;

        var handler = new FakeHttpMessageHandler(_ =>
        {
            callCount++;

            if (callCount == 1)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("invalid json {{["),
                };
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """
                    [
                        {
                            "type": "energy",
                            "name": "Kitchen",
                            "payload": { "value": 20 }
                        }
                    ]
                    """,
                    Encoding.UTF8,
                    "application/json"),
            };
        });

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://fake-api"),
        };

        var client = new WeakAppApiClient(
            httpClient,
            NullLogger<WeakAppApiClient>.Instance);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        callCount.Should().Be(2);
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldHandleMixedValidAndInvalidItems()
    {
        // Arrange
        var json = """
                   [
                       { "type": "unknown_type", "name": "Living Room", "payload": {} },
                       { "type": "energy", "name": "Unknown Place", "payload": {} },
                       { "type": "energy", "name": "Kitchen", "payload": { "value": 25 } },
                       { "type": "humidity" },
                       { "type": "motion", "name": "Office", "payload": { "detected": true } }
                   ]
                   """;

        var client = CreateClient(json);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].SensorType.Should().Be(SensorType.Energy);
        result[0].LocationType.Should().Be(LocationType.Kitchen);
        result[1].SensorType.Should().Be(SensorType.Motion);
        result[1].LocationType.Should().Be(LocationType.Office);
    }

    [Fact]
    public async Task GetSensorDataAsync_ShouldPreserveComplexPayload()
    {
        // Arrange
        var json = """
                   [
                       {
                           "type": "energy",
                           "name": "Living Room",
                           "payload": {
                               "voltage": 230.5,
                               "current": 12.3,
                               "power_factor": 0.95,
                               "readings": [10.5, 11.2, 10.8]
                           }
                       }
                   ]
                   """;

        var client = CreateClient(json);

        // Act
        var result = await client.GetSensorDataAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var payload = result[0].Payload;
        payload.GetProperty("voltage").GetDouble().Should().Be(230.5);
        payload.GetProperty("current").GetDouble().Should().Be(12.3);
        payload.GetProperty("power_factor").GetDouble().Should().Be(0.95);
        payload.GetProperty("readings").EnumerateArray().Should().HaveCount(3);
    }

    private static WeakAppApiClient CreateClient(string responseJson, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var handler = new FakeHttpMessageHandler(_ =>
            new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json"),
            });

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://fake-api"),
        };

        return new WeakAppApiClient(
            httpClient,
            NullLogger<WeakAppApiClient>.Instance);
    }
}

internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

    public FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        _handler = handler;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_handler(request));
    }
}
