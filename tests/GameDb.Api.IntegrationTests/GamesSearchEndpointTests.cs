using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using GameDb.Api.Authorization;
using GameDb.Api.Contracts.PriorityRead;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GameDb.Api.IntegrationTests;

public sealed class GamesSearchEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GamesSearchEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Testing"));
    }

    [Fact]
    public async Task Search_ReturnsUnauthorized_WhenTokenMissing()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/v1/games/search?query=chrono");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Search_ReturnsBadRequest_WhenQueryMissing()
    {
        var client = _factory.CreateClient();
        AttachBearerToken(client, RoleNames.Member);

        var response = await client.GetAsync("/api/v1/games/search");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Search_UsesBotPageSize_WhenBothPageSizeAndBotPageSizeProvided()
    {
        var client = _factory.CreateClient();
        AttachBearerToken(client, RoleNames.ServiceBot);

        var response = await client.GetAsync("/api/v1/games/search?query=chrono&pageSize=1&botPageSize=2");
        var payload = await response.Content.ReadFromJsonAsync<GameSearchResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal(2, payload.PageSize);
        Assert.Equal(2, payload.Items.Count);
    }

    private void AttachBearerToken(HttpClient client, string role)
    {
        var token = TestJwtTokenFactory.CreateToken(_factory.Services, role);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
