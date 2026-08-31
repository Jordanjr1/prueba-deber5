using Yarp.ReverseProxy.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen; // O asegúrate de tener este using:
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;

namespace ApiGateway_Vehiculos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de YARP
            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            // Servicios para Swagger / OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configurar el middleware de Swagger para que abra directamente en la raíz (http://localhost:5000/)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway Vehiculos V1");
                c.RoutePrefix = string.Empty;
            });

            app.MapGet("/", () => "API Gateway activo!");

            // Mapear el Reverse Proxy de YARP
            app.MapReverseProxy();

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