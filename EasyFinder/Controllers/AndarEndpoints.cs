using EasyFinder.DbConfig;
using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;

public static class AndarEndpoints
{
    public static void Map(RouteGroupBuilder group)
    {

        group.MapGroup("/andares").WithTags("Andar").RequireAuthorization();
        
        // Get all
        group.MapGet("/andares", async (MottuDbContext db) =>
            await db.Andares.ToListAsync())
            .Produces<Andar>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Retorna todos os andares")
            .WithDescription("Retorna todos os andares  cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado um andar, ele ainda vai retornar uma lista");

        // GetById
        group.MapGet("/andares/{id}", async (int id, MottuDbContext db) =>
        {
            var andar = await db.Andares.FindAsync(id);
            return andar is not null ? Results.Ok(andar) : Results.NotFound();
        })
        .Produces<Andar>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Busca um andar pelo ID")
        .WithDescription("Retorna os dados de um andar específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/andares/inserir", async (Andar andar, MottuDbContext db) =>
        {
            if (andar == null)
                return Results.BadRequest("Dados inválidos.");
            
            db.Andares.Add(andar);
            await db.SaveChangesAsync();
            return Results.Created($"/andares/{andar.Id}", andar);
        })
        .Produces<Andar>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Accepts<Andar>("application/json")
        .WithSummary("Insere um novo andar")
        .WithDescription("Adiciona um novo andar ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/andares/atualizar/{id}", async (int id, Andar andar, MottuDbContext db) =>
        {
            var existing = await db.Andares.FindAsync(id);
            if (existing == null) 
                return Results.NotFound();

            existing.Numero_andar = andar.Numero_andar;
            await db.SaveChangesAsync();

            return Results.Ok($"Andar com ID {id} atualizado com sucesso.");
        })
        .Produces<Andar>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Accepts<Andar>("application/json")
        .WithSummary("Atualiza um andar existente")
        .WithDescription("Atualiza os dados de um andar já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/andares/deletar/{id}", async (int id, MottuDbContext db) =>
            {
                var andar = await db.Andares.FindAsync(id);
                if (andar == null) 
                    return Results.NotFound();

                db.Andares.Remove(andar);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Andar com ID {id} removido com sucesso.");
            })
            .Produces<Andar>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Remove um andar")
        .WithDescription("Remove um andar do banco de dados com base no ID informado. " +
                         "Caso o andar não seja encontrado, retorna 404 Not Found.");
    }
}