using Microsoft.EntityFrameworkCore;
using Microservicio_Auth.Models;

namespace Microservicio_Auth.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeo explícito de la clave primaria
        modelBuilder.Entity<Usuario>()
            .HasKey(u => u.IdUsuario);
    }
}