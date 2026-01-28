namespace GraphQLGateway.Api.IntegrationTests.QueriesTests;

using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using GraphQLGateway.Api.IntegrationTests.Infrastructure;

public class AirQualityReadingsHttpTests : IClassFixture<GraphqlWebAppFactory>
{
    private readonly HttpClient _client;

    public AirQualityReadingsHttpTests(GraphqlWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Query_ReturnsData_FromInMemoryDatabase()
    {
        // Arrange
        var request = new
        {
            query = """
                    query {
                        airQualityReadings {
                            nodes {
                                id
                                co2
                                pm25
                                humidity
                            }
                        }
                    }
                    """,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/graphql", request);

        // Assert
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        var nodes = doc.RootElement.GetProperty("data")
            .GetProperty("airQualityReadings")
            .GetProperty("nodes");

        nodes.GetArrayLength().Should().Be(2);

        var firstNode = nodes[0];
        firstNode.GetProperty("co2").GetInt32().Should().Be(100);
        firstNode.GetProperty("pm25").GetInt32().Should().Be(50);
        firstNode.GetProperty("humidity").GetInt32().Should().Be(25);
    }
}
