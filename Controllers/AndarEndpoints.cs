using EasyFinder.DbConfig;
using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;

public static class AndarEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/andares").WithTags("Andar");
        
        // Get all
        group.MapGet("/", async (MottuDbContext db) =>
            await db.Andares.ToListAsync())
            .WithSummary("Retorna todos os andares")
            .WithDescription("Retorna todos os andares  cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um andar, ele ainda vai retornar uma lista");

        // GetById
        group.MapGet("/{id}", async (int id, MottuDbContext db) =>
        {
            var andar = await db.Andares.FindAsync(id);
            return andar is not null ? Results.Ok(andar) : Results.NotFound();
        })
        .WithSummary("Busca um andar pelo ID")
        .WithDescription("Retorna os dados de um andar específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/", async (Andar andar, MottuDbContext db) =>
        {
            db.Andares.Add(andar);
            await db.SaveChangesAsync();
            return Results.Created($"/andares/{andar.Id}", andar);
        })
        .WithSummary("Insere um novo andar")
        .WithDescription("Adiciona um novo andar ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/{id}", async (int id, Andar andar, MottuDbContext db) =>
        {
            var existing = await db.Andares.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Numero_andar = andar.Numero_andar;
            await db.SaveChangesAsync();

            return Results.Ok($"Andarle com ID {id} atualizado com sucesso.");
        })
        .WithSummary("Atualiza um andar existente")
        .WithDescription("Atualiza os dados de um andar já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, MottuDbContext db) =>
            {
                var andar = await db.Andares.FindAsync(id);
                if (andar is null) return Results.NotFound();

                db.Andares.Remove(andar);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Andar com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove um andar")
        .WithDescription("Remove um andar do banco de dados com base no ID informado. " +
                         "Caso o andar não seja encontrado, retorna 404 Not Found.");
    }
}