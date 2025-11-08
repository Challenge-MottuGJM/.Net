using EasyFinder.DbConfig;
using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;

public static class VagaEndpoints
{
    public static void Map(RouteGroupBuilder group)
    {

        group.MapGroup("/vagas").WithTags("Vaga").RequireAuthorization();
        
        //Get all
        group.MapGet("/vagas", async (MottuDbContext db) =>
            await db.Vagas.ToListAsync())
            .Produces<Vaga>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Retorna todos as vagas")
            .WithDescription("Retorna todos as vagas cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado uma vaga, ele ainda vai retornar uma lista");

        //GetById
        group.MapGet("/vagas/{id}", async (int id, MottuDbContext db) =>
        {
            var vaga = await db.Vagas.FindAsync(id);
            return vaga is not null ? Results.Ok(vaga) : Results.NotFound();
        })
        .Produces<Vaga>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Busca uma vaga pelo ID")
        .WithDescription("Retorna os dados de uma vaga específico com base no ID informado. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Inserir
        group.MapPost("/vagas/inserir", async (Vaga vaga, MottuDbContext db) =>
        {
            if (vaga == null)
                return Results.BadRequest("Dados inválidos.");
            
            db.Vagas.Add(vaga);
            await db.SaveChangesAsync();
            return Results.Created($"/Vagas/{vaga.Id}", vaga);
        })
        .Produces<Vaga>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Accepts<Vaga>("application/json")
        .WithSummary("Insere uma nova vaga")
        .WithDescription("Adiciona uma nova vaga ao banco de dados com base nos dados enviados no corpo da requisição.");
        
        // Atualizar
        group.MapPut("/vagas/atualizar/{id}", async (int id, Vaga vaga, MottuDbContext db) =>
        {
            var existing = await db.Vagas.FindAsync(id);
            if (existing == null) 
                return Results.NotFound();

            existing.Numero_vaga = vaga.Numero_vaga;
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .Produces<Vaga>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Accepts<Vaga>("application/json")
        .WithSummary("Atualiza uma vaga existente")
        .WithDescription("Atualiza os dados de uma vaga já cadastrado, identificado pelo ID. " +
                         "Caso o ID não exista, retorna 404 Not Found.");
        
        // Deletar
        group.MapDelete("/vagas/deletar/{id}", async (int id, MottuDbContext db) =>
        {
            var vaga = await db.Vagas.FindAsync(id);
            if (vaga == null) 
                return Results.NotFound();

            db.Vagas.Remove(vaga);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .Produces<Vaga>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Remove uma vaga")
        .WithDescription("Remove uma vaga do banco de dados com base no ID informado. " +
                         "Caso a vaga não seja encontrado, retorna 404 Not Found.");
    }
}