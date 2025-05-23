using EasyFinder.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyFinder.DbConfig;

public class MottuDbContext :DbContext
{
    
    public MottuDbContext(DbContextOptions<MottuDbContext> options) : base(options) { }
    
    public DbSet<Bloco> Blocos { get; set; }
    public DbSet<Andar> Andares { get; set; }
    public DbSet<Galpao> Galpoes { get; set; }
    public DbSet<Patio> Patios { get; set; }
    public DbSet<Vaga> Vagas { get; set; }
    public DbSet<Moto> Motos { get; set; }
}   