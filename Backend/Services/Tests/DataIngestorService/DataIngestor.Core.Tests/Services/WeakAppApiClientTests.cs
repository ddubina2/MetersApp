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
