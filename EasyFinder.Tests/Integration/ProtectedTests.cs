using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

public class ProtectedTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public ProtectedTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Sem_Token_Deve_Retornar_401()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/api/v1/motos");
        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    [Fact]
    public async Task Com_Token_Deve_Retornar_200()
    {
        var client = _factory.CreateClient();
        var login = await client.PostAsJsonAsync("/login", new { usuario = "admin", senha = "senha123" });
        login.EnsureSuccessStatusCode();
        var json = await login.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        var token = json!["token"];

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var resp = await client.GetAsync("/api/v1/motos");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }
}