using EasyFinder.DbConfig;
using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;


public static class BlocoEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/blocos").WithTags("Bloco");
        
        //Get all
        group.MapGet("/", async (MottuDbContext db) =>
            await db.Blocos.ToListAsync())
            .WithSummary("Retorna todos os bloco")
            .WithDescription("Retorna todos os blocos cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um bloco, ele ainda vai retornar uma lista");

        //GetById
        group.MapGet("/{id}", async (int id, MottuDbContext db) =>
        {
            var bloco = await db.Blocos.FindAsync(id);
            return bloco is not null ? Results.Ok(bloco) : Results.NotFound();
        })
        .WithSummary("Busca um bloco pelo ID")
        .WithDescription("Retorna os dados de um bloco específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/", async (Bloco bloco, MottuDbContext db) =>
        {
            db.Blocos.Add(bloco);
            await db.SaveChangesAsync();
            return Results.Created($"/Blocos/{bloco.Id}", bloco);
        })
        .WithSummary("Insere um novo bloco")
        .WithDescription("Adiciona um novo bloco ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/{id}", async (int id, Bloco bloco, MottuDbContext db) =>
        {
            var existing = await db.Blocos.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Letra_bloco = bloco.Letra_bloco;
            await db.SaveChangesAsync();

            return Results.Ok($"Bloco com ID {id} atualizado com sucesso.");
        })
        .WithSummary("Atualiza um bloco existente")
        .WithDescription("Atualiza os dados de um bloco já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, MottuDbContext db) =>
            {
                var bloco = await db.Blocos.FindAsync(id);
                if (bloco is null) return Results.NotFound();

                db.Blocos.Remove(bloco);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Bloco com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove um bloco")
        .WithDescription("Remove um bloco do banco de dados com base no ID informado. " +
                         "Caso o bloco não seja encontrado, retorna 404 Not Found.");
    }
}