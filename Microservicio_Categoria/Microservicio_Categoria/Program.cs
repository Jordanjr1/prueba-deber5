using Microservicio_Categoria.Models;
using Microservicio_Categoria.Services; // <-- Necesario para reconocer IRabbitMQProducer y RabbitMQProducer
using Microsoft.EntityFrameworkCore;

namespace Microservicio_Categoria
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

            // Registro del Producer de RabbitMQ para enviar eventos al crear categorías
            builder.Services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

            // Servicios para Swagger / OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configurar el middleware de Swagger para que abra directamente en la raíz (ej: http://localhost:5001/)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservicio Categoria V1");
                c.RoutePrefix = string.Empty;
            });

            // Asegurar que la base de datos se cree automáticamente al iniciar
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}