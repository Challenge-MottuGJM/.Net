using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using EasyFinder.DbConfig;  
using EasyFinder.Model;      

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this IEndpointRouteBuilder app, string jwtKey, string jwtIssuer)
    {
        app.MapPost("/login", async (LoginDto login, MottuDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(login.Usuario) || string.IsNullOrWhiteSpace(login.Senha))
                return Results.BadRequest(new { error = "Usuario e senha são obrigatórios." });

            // Busca usuário por nome de usuario
            var user = await db.Set<Usuario>()
                               .AsNoTracking()
                               .FirstOrDefaultAsync(u => u.Username == login.Usuario);

            if (user is null)
                return Results.Unauthorized();
            
            bool senhaValida = user.Senha == login.Senha;

            if (!senhaValida)
                return Results.Unauthorized();
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(new { token = tokenString });
        })
        .AllowAnonymous()
        .WithSummary("Login")
        .WithDescription("Autentica o usuário consultando o banco e retorna um JWT válido por 2 horas.");
    }
}

public record LoginDto(string Usuario, string Senha);
