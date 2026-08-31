using Microsoft.EntityFrameworkCore;

namespace Microservicio_Categoria.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Aquí le decimos a Entity Framework que cree una tabla llamada "Categorias" basada en tu modelo
        public DbSet<Categoria> Categorias { get; set; }
    }
}