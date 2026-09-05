using Microservicio_Auth.Data;
using Microservicio_Auth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrar Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configurar la cadena de conexión y DbContext para SQL Server
var connectionString = builder.Configuration.GetConnectionString("ConexionSql")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Inyección de Dependencias
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// 4. Configurar Pipeline de peticiones
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API v1");
        c.RoutePrefix = string.Empty; // Hace que Swagger sea la página principal
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();