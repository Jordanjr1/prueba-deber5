using Microservicio_Auth.Services;
using Microsoft.AspNetCore.Builder;


var builder = WebApplication.CreateBuilder(args);

// 1. Registrar Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Inyección de Dependencias
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// 3. Configurar Pipeline de peticiones
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API v1");
        c.RoutePrefix = string.Empty; // Hace que Swagger sea la página principal
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();