using EasyFinder.DbConfig;
using EasyFinder.Model;
using EasyFinder.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.Controllers;


public static class MotoEndpoints
{
    
    private const string Status404 = "Caso o ID não exista, retorna 404 Not Found.";
    
    public static void Map(WebApplication app)
    {

        var group = app.MapGroup("/motos").WithTags("Moto");
        
        // Get all
        group.MapGet("/", async (MottuDbContext db) =>
            await db.Motos.ToListAsync())
            .WithSummary("Retorna todas as motos")
            .WithDescription("Retorna todas as motos cadastrados no banco de dados, " +
                             "mesmo que só seja encontrado uma vaga, ele ainda vai retornar uma lista");

        // GetById
        group.MapGet("/{id}", async (int id, MottuDbContext db) =>
        {
            var moto = await db.Motos.FindAsync(id);
            return moto is not null ? Results.Ok(moto) : Results.NotFound();
        })
        .WithSummary("Busca uma vaga pelo ID")
        .WithDescription("Retorna os dados de uma vaga específico com base no ID informado. " +
                         Status404);
        
        //Get por status
        group.MapGet("/status/{status}", async (string status, MottuDbContext db) =>
        {
            var motos = await db.Motos
                .Where(m => string.Equals(m.Status, status, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return motos.Count > 0 ? Results.Ok(motos) : Results.NotFound();
        })
        .WithSummary("Busca uma vaga pelo Status")
        .WithDescription("Retorna os dados de uma vaga específico com base no Status informado. " +
                         Status404);
        
        //Get por modelo
        group.MapGet("/modelo/{modelo}", async (string modelo, MottuDbContext db) =>
        {
            var motos = await db.Motos
                .Where(m => string.Equals(m.Modelo, modelo, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return motos.Count > 0 ? Results.Ok(motos) : Results.NotFound();
        })
        .WithSummary("Busca uma vaga pelo modelo")
        .WithDescription("Retorna os dados de uma vaga específico com base no modelo informado. " +
                         Status404);
        
        //Get por placa
        group.MapGet("/placa/{placa}", async (string placa, MottuDbContext db) =>
        {
            var motos = await db.Motos
                .Where(m => string.Equals(m.Placa, placa, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return motos.Count > 0 ? Results.Ok(motos) : Results.NotFound();
        })
        .WithSummary("Busca uma vaga pela placa")
        .WithDescription("Retorna os dados de uma vaga específico com base na placa informado. " +
                         Status404);
        
        // Busca paginada por modelo
        group.MapGet("/search", async (int? page, string? modelo, MottuDbContext db) =>
            {
                var pageSize = 10;
                var skipItems = page.HasValue ? (page.Value - 1) * pageSize : 0;
                var searchTerm = modelo ?? string.Empty;

                var query = db.Motos
                    .Where(m => m.Modelo.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

                var totalItems = await query.CountAsync();
                var data = await query
                    .Skip(skipItems)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new SearchDto<Moto>(searchTerm, page, totalItems, data));
            })
            .WithSummary("Busca paginada de motos por modelo")
            .WithDescription("Retorna uma lista paginada de motos cujo modelo contenha o termo informado. " +
                             "Aceita parâmetros opcionais de modelo e página.");
        
        // Busca paginada
        group.MapGet("/paginadas", async (int? page, MottuDbContext db) =>
            {
                var pageSize = 10;
                var currentPage = page ?? 1;
                var skipItems = (currentPage - 1) * pageSize;

                var totalItems = await db.Motos.CountAsync();
                var data = await db.Motos
                    .Skip(skipItems)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new SearchDto<Moto>(null, currentPage, totalItems, data));
            })
            .WithSummary("Retorna motos paginadas")
            .WithDescription("Retorna todos os registros de motos paginados. " +
                             "Cada página retorna um número fixo de registros (10 por página neste exemplo).");

        
        // Inserir
        group.MapPost("/", async (Moto moto, MottuDbContext db) =>
        {
            if (moto == null)
                return Results.BadRequest("Dados inválidos.");
            
            db.Motos.Add(moto);
            await db.SaveChangesAsync();
            return Results.Created($"/motos/{moto.Id}", moto);
        })
        .WithSummary("Insere uma nova moto")
        .WithDescription("Adiciona uma nova moto ao banco de dados com base nos dados enviados no corpo da requisição.");

        
        // Atualizar
        group.MapPut("/{id}", async (int id, Moto moto, MottuDbContext db) =>
        {
            var existing = await db.Motos.FindAsync(id);
            if (existing == null) 
                return Results.NotFound();

            existing.Chassi = moto.Chassi;
            existing.Status = moto.Status;
            existing.Marca = moto.Marca;
            existing.Placa = moto.Placa;
            existing.Modelo = moto.Modelo;
            existing.Vaga_id = moto.Vaga_id;
            
            await db.SaveChangesAsync();

            return Results.Ok($"Moto com ID {id} atualizada com sucesso.");
        })
        .WithSummary("Atualiza uma moto existente")
        .WithDescription("Atualiza os dados de uma moto já cadastrado, identificado pelo ID. " +
                         Status404);
        
        // Deletar
        group.MapDelete("/deletar/{id}", async (int id, MottuDbContext db) =>
            {
                var moto = await db.Motos.FindAsync(id);
                if (moto == null) 
                    return Results.NotFound();

                db.Motos.Remove(moto);
                await db.SaveChangesAsync();
                
                return Results.Ok($"Moto com ID {id} removido com sucesso.");
            })
        .WithSummary("Remove uma moto")
        .WithDescription("Remove uma moto do banco de dados com base no ID informado. " +
                         "Caso a moto não seja encontrado, retorna 404 Not Found.");
    }
}