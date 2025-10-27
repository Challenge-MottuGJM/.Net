using EasyFinder.DbConfig;
using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;

public static class GalpaoEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/galpoes").WithTags("Galpão");
        
        //Get all
        group.MapGet("/", async (MottuDbContext db) =>
            await db.Galpoes.ToListAsync())
            .WithSummary("Retorna todos os galpões")
            .WithDescription("Retorna todos os galpões cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um galpão, ele ainda vai retornar uma lista");

        //GetById
        group.MapGet("/{id}", async (int id, MottuDbContext db) =>
        {
            var galpao = await db.Galpoes.FindAsync(id);
            return galpao is not null ? Results.Ok(galpao) : Results.NotFound();
        })
        .WithSummary("Busca um galpão pelo ID")
        .WithDescription("Retorna os dados de um galpão específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/inserir", async (Galpao galpao, MottuDbContext db) =>
            {
                if (galpao == null)
                    return Results.BadRequest("Dados inválidos.");
                
                db.Galpoes.Add(galpao);
                await db.SaveChangesAsync();
                return Results.Created($"/Galpoes/{galpao.Id}", galpao);
            })
            .WithSummary("Insere um novo galpão")
            .WithDescription("Adiciona um novo galpão ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/atualizar/{id}", async (int id, Galpao galpao, MottuDbContext db) =>
        {
            var existing = await db.Galpoes.FindAsync(id);
            if (existing == null) 
                return Results.NotFound();

            existing.Nome_galpao = galpao.Nome_galpao;
            await db.SaveChangesAsync();

            return Results.Ok($"Galpão com ID {id} atualizado com sucesso.");
        })
        .WithSummary("Atualiza um galpão existente")
        .WithDescription("Atualiza os dados de um galpão já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, MottuDbContext db) =>
            {
                var galpao = await db.Galpoes.FindAsync(id);
                if (galpao == null) 
                    return Results.NotFound();

                db.Galpoes.Remove(galpao);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Galpão com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove um galpão")
        .WithDescription("Remove um galpão do banco de dados com base no ID informado. " +
                         "Caso o galpão não seja encontrado, retorna 404 Not Found.");
    }
}