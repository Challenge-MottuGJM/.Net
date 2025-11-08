using EasyFinder.DbConfig;
using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;

public static class PatioEndpoints
{
    public static void Map(RouteGroupBuilder group)
    {

        group.MapGroup("/patios").WithTags("Patio").RequireAuthorization();
        
        //Get all
        group.MapGet("/patios", async (MottuDbContext db) =>
            await db.Patios.ToListAsync())
            .Produces<Patio>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Retorna todos os patios")
            .WithDescription("Retorna todos os patios cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um patio, ele ainda vai retornar uma lista");

        //GetById
        group.MapGet("/patios/{id}", async (int id, MottuDbContext db) =>
        {
            var patio = await db.Patios.FindAsync(id);
            return patio is not null ? Results.Ok(patio) : Results.NotFound();
        })
        .Produces<Patio>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Busca um patio pelo ID")
        .WithDescription("Retorna os dados de um patio específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/patios/inserir", async (Patio patio, MottuDbContext db) =>
        {
            if (patio == null)
                return Results.BadRequest("Dados inválidos.");
            
            db.Patios.Add(patio);
            await db.SaveChangesAsync();
            return Results.Created($"/Patios/{patio.Id}", patio);
        })
        .Produces<Patio>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Accepts<Patio>("application/json")
        .WithSummary("Insere um novo patio")
        .WithDescription("Adiciona um novo patio ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/patios/atualizar/{id}", async (int id, Patio patio, MottuDbContext db) =>
        {
            var existing = await db.Patios.FindAsync(id);
            if (existing == null) 
                return Results.NotFound();

            existing.Numero_patio = patio.Numero_patio;
            await db.SaveChangesAsync();

            return Results.Ok($"Patio com ID {id} atualizado com sucesso.");
        })
        .Produces<Patio>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Accepts<Patio>("application/json")
        .WithSummary("Atualiza um patio existente")
        .WithDescription("Atualiza os dados de um patio já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/patios/deletar/{id}", async (int id, MottuDbContext db) =>
            {
                var patio = await db.Patios.FindAsync(id);
                if (patio == null) 
                    return Results.NotFound();

                db.Patios.Remove(patio);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Patio com ID {id} removido com sucesso.");
            })
            .Produces<Patio>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Remove um patio")
        .WithDescription("Remove um patio do banco de dados com base no ID informado. " +
                         "Caso o patio não seja encontrado, retorna 404 Not Found.");
    }
}