using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using Xunit;

namespace EasyFinder.Tests.Integration;

public class ApiAuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public ApiAuthTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Login_Deve_Retornar_Token()
    {
        var client = _factory.CreateClient();
        var resp = await client.PostAsJsonAsync("/login", new { usuario = "admin", senha = "senha123" });
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(json);
        Assert.True(json!.ContainsKey("token"));
        Assert.False(string.IsNullOrWhiteSpace(json["token"]));
    }
}