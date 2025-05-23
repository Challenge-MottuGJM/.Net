using EasyFinder.DbConfig;
using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;

public class PatioEndpoints
{
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/patios").WithTags("Patio");
        
        //Get all
        group.MapGet("/", async (MottuDbContext db) =>
            await db.Patios.ToListAsync())
            .WithSummary("Retorna todos os patios")
            .WithDescription("Retorna todos os patios cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um patio, ele ainda vai retornar uma lista");

        //GetById
        group.MapGet("/{id}", async (int id, MottuDbContext db) =>
        {
            var patio = await db.Patios.FindAsync(id);
            return patio is not null ? Results.Ok(patio) : Results.NotFound();
        })
        .WithSummary("Busca um patio pelo ID")
        .WithDescription("Retorna os dados de um patio específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/", async (Patio patio, MottuDbContext db) =>
        {
            db.Patios.Add(patio);
            await db.SaveChangesAsync();
            return Results.Created($"/Patios/{patio.Id}", patio);
        })
        .WithSummary("Insere um novo patio")
        .WithDescription("Adiciona um novo patio ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/{id}", async (int id, Patio patio, MottuDbContext db) =>
        {
            var existing = await db.Patios.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Numero_patio = patio.Numero_patio;
            await db.SaveChangesAsync();

            return Results.Ok($"Patio com ID {id} atualizado com sucesso.");
        })
        .WithSummary("Atualiza um patio existente")
        .WithDescription("Atualiza os dados de um patio já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, MottuDbContext db) =>
            {
                var patio = await db.Patios.FindAsync(id);
                if (patio is null) return Results.NotFound();

                db.Patios.Remove(patio);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Patio com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove um patio")
        .WithDescription("Remove um patio do banco de dados com base no ID informado. " +
                         "Caso o patio não seja encontrado, retorna 404 Not Found.");
    }
}