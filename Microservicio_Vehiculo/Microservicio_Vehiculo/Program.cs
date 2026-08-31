using Microservicio_Vehiculo.Models;
using Microservicio_Vehiculo.Events;
using Microservicio_Vehiculo.Services; // <-- ¡Agrega esta línea!
using Microsoft.EntityFrameworkCore;

namespace Microservicio_Vehiculo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de la base de datos SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllers();

            // Agrega esto en tu Program.cs junto a tus demás servicios
            builder.Services.AddHostedService<RabbitMQConsumer>();

            // Servicios para Swagger / OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configurar el middleware de Swagger para que abra directamente en la raíz (ej: http://localhost:5002/)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservicio Vehiculo V1");
                c.RoutePrefix = string.Empty;
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Asegurar que la base de datos se cree automáticamente al iniciar
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}