using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EasyFinder.Tests.Integration;

public class MlManutencaoTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    public MlManutencaoTests(CustomWebApplicationFactory factory) => _factory = factory;

    [Fact]
    public async Task Deve_Retornar_Probabilidade()
    {
        
        var client = _factory.CreateClient();
        client.Timeout = TimeSpan.FromMinutes(10); 

        var login = await client.PostAsJsonAsync("/login", new { usuario = "admin", senha = "senha123" });
        login.EnsureSuccessStatusCode();

        var token = (await login.Content.ReadFromJsonAsync<Dictionary<string, string>>())!["token"];
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var body = new
        {
            kmDesdeUltimaRevisao = 3200,
            diasDesdeUltimaRevisao = 75,
            incidentesUltimos30d = 1,
            usoDiarioMedioHoras = 6,
            idadeEmMeses = 28,
            velocidadeMedia = 42,
            trocasOleoAtrasadas = 1
        };

        var resp = await client.PostAsJsonAsync("/api/v1/ml/motos/9C2KC0810FR123456/prob-manutencao", body);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        Assert.NotNull(json);
        Assert.True(json!.ContainsKey("probabilidade"));
    }
}