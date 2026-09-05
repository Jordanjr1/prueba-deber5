using Microservicio_Auth.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Microservicio_Auth.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
}