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
    
    public DbSet<Usuario> Usuarios { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("USUARIO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID");
            e.Property(x => x.Username).HasColumnName("USUARIO").HasMaxLength(100).IsRequired();
            e.Property(x => x.Senha).HasColumnName("SENHA").HasMaxLength(200).IsRequired();
        });
    }
}   