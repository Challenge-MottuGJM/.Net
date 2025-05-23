using EasyFinder.Controllers;
using EasyFinder.DbConfig;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MottuDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("FiapOracleDb")));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.MapScalarApiReference(); 
}

GalpaoEndpoints.Map(app);
PatioEndpoints.Map(app);
AndarEndpoints.Map(app);
BlocoEndpoints.Map(app);
VagaEndpoints.Map(app);
MotoEndpoints.Map(app);

app.Run();