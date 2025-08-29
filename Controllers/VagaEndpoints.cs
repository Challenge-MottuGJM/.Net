using EasyFinder.DbConfig;
using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;

public static class VagaEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/vagas").WithTags("Vaga");
        
        //Get all
        group.MapGet("/", async (MottuDbContext db) =>
            await db.Vagas.ToListAsync())
            .WithSummary("Retorna todos as vagas")
            .WithDescription("Retorna todos as vagas cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado uma vaga, ele ainda vai retornar uma lista");

        //GetById
        group.MapGet("/{id}", async (int id, MottuDbContext db) =>
        {
            var vaga = await db.Vagas.FindAsync(id);
            return vaga is not null ? Results.Ok(vaga) : Results.NotFound();
        })
        .WithSummary("Busca uma vaga pelo ID")
        .WithDescription("Retorna os dados de uma vaga específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/", async (Vaga vaga, MottuDbContext db) =>
        {
            db.Vagas.Add(vaga);
            await db.SaveChangesAsync();
            return Results.Created($"/Vagas/{vaga.Id}", vaga);
        })
        .WithSummary("Insere uma nova vaga")
        .WithDescription("Adiciona uma nova vaga ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/{id}", async (int id, Vaga vaga, MottuDbContext db) =>
        {
            var existing = await db.Vagas.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Numero_vaga = vaga.Numero_vaga;
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithSummary("Atualiza uma vaga existente")
        .WithDescription("Atualiza os dados de uma vaga já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/{id}", async (int id, MottuDbContext db) =>
        {
            var vaga = await db.Vagas.FindAsync(id);
            if (vaga is null) return Results.NotFound();

            db.Vagas.Remove(vaga);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithSummary("Remove uma vaga")
        .WithDescription("Remove uma vaga do banco de dados com base no ID informado. " +
                         "Caso a vaga não seja encontrado, retorna 404 Not Found.");
    }
}