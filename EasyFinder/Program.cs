using System.Text;
using EasyFinder.Controllers;
using EasyFinder.DbConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

public class Program
{
    public static async Task Main(string[] args)
    {

        var jwtKey = "p1E4QW6L0wSYd5y2rVYj3x8K9nM2tB0h7uC4zP1qR6sT8wZ3aJ5dF6gH8kL2mN";
        var jwtIssuer = "EasyFinderAPI";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<MottuDbContext>(options =>
            options.UseOracle(builder.Configuration.GetConnectionString("FiapOracleDb")));

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new();
                document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();
                document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "JWT Bearer token"
                };

                document.SecurityRequirements ??= new List<OpenApiSecurityRequirement>();
                document.SecurityRequirements.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }
                    ] = Array.Empty<string>()
                });

                return Task.CompletedTask;
            });
        });

        builder.Services.AddHealthChecks();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.MapHealthChecks("/health");

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.WithPreferredScheme("Bearer");
                options.WithHttpBearerAuthentication(bearer => { });
                options.Authentication = new ScalarAuthenticationOptions
                {
                    PreferredSecurityScheme = "Bearer"
                };
            });
        }

// Endpoint de login (gera JWT)
        LoginEndpoints.MapLoginEndpoints(app, jwtKey, jwtIssuer);

        app.UseAuthentication();
        app.UseAuthorization();

// Grupo versionado protegido
        var v1 = app.MapGroup("/api/v1").RequireAuthorization();

// Endpoints protegidos
        GalpaoEndpoints.Map(v1);
        PatioEndpoints.Map(v1);
        AndarEndpoints.Map(v1);
        BlocoEndpoints.Map(v1);
        VagaEndpoints.Map(v1);
        MotoEndpoints.Map(v1);

        await app.RunAsync();
    }
}