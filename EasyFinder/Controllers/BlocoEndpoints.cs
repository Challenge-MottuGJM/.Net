using EasyFinder.DbConfig;
using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;


public static class BlocoEndpoints
{
    public static void Map(RouteGroupBuilder group)
    {

        group.MapGroup("/blocos").WithTags("Bloco").RequireAuthorization();
        
        //Get all
        group.MapGet("/blocos", async (MottuDbContext db) =>
            await db.Blocos.ToListAsync())
            .Produces<Bloco>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Retorna todos os bloco")
            .WithDescription("Retorna todos os blocos cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um bloco, ele ainda vai retornar uma lista");

        //GetById
        group.MapGet("/blocos/{id}", async (int id, MottuDbContext db) =>
        {
            var bloco = await db.Blocos.FindAsync(id);
            return bloco is not null ? Results.Ok(bloco) : Results.NotFound();
        })
        .Produces<Bloco>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Busca um bloco pelo ID")
        .WithDescription("Retorna os dados de um bloco específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/blocos/inserir", async (Bloco bloco, MottuDbContext db) =>
        {
            if (bloco == null)
                return Results.BadRequest("Dados inválidos.");
            
            db.Blocos.Add(bloco);
            await db.SaveChangesAsync();
            return Results.Created($"/Blocos/{bloco.Id}", bloco);
        })
        .Produces<Bloco>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Accepts<Bloco>("application/json")
        .WithSummary("Insere um novo bloco")
        .WithDescription("Adiciona um novo bloco ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/blocos/atualizar/{id}", async (int id, Bloco bloco, MottuDbContext db) =>
        {
            var existing = await db.Blocos.FindAsync(id);
            if (existing == null)
                return Results.NotFound();

            existing.Letra_bloco = bloco.Letra_bloco;
            await db.SaveChangesAsync();

            return Results.Ok($"Bloco com ID {id} atualizado com sucesso.");
        })
        .Produces<Bloco>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Accepts<Bloco>("application/json")
        .WithSummary("Atualiza um bloco existente")
        .WithDescription("Atualiza os dados de um bloco já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/blocos/deletar/{id}", async (int id, MottuDbContext db) =>
            {
                var bloco = await db.Blocos.FindAsync(id);
                if (bloco == null) 
                    return Results.NotFound();

                db.Blocos.Remove(bloco);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Bloco com ID {id} removido com sucesso.");
            })
            .Produces<Bloco>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Remove um bloco")
            .WithDescription("Remove um bloco do banco de dados com base no ID informado. " +
                             "Caso o bloco não seja encontrado, retorna 404 Not Found.");
    }
}