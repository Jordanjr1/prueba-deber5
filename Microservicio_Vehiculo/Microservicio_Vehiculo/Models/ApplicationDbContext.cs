using Microsoft.EntityFrameworkCore;

namespace Microservicio_Vehiculo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Vehiculo> Vehiculos { get; set; }
    }
}