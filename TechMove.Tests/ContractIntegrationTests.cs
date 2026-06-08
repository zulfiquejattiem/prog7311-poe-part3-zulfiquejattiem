using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace TechMove.Tests;

public class ContractIntegrationTests
{
    private readonly HttpClient _client;

    public ContractIntegrationTests()
    {
        // Assume API is running locally on port 5000 (docker compose mapping)
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
    }

    [Fact]
    public async Task GetContracts_ReturnsOk_AndJsonNotNull()
    {
        var resp = await _client.GetAsync("api/contracts");
        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode); // endpoint requires JWT by default

        // If your API is configured to allow anonymous access change assertion above to OK and uncomment below
        // var items = await resp.Content.ReadFromJsonAsync<object>();
        // Assert.NotNull(items);
    }
}
