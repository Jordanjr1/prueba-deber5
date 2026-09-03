using Microsoft.AspNetCore.Mvc;
using Microservicio_Auth.Services;

namespace Microservicio_Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        // Validación simulada de usuario y contraseña (puedes conectarlo a SQL Server luego)
        if (dto.Usuario == "admin" && dto.Password == "123456")
        {
            var token = _jwtService.GenerarToken(dto.Usuario);
            return Ok(new { token, mensaje = "Autenticación exitosa" });
        }

        return Unauthorized(new { mensaje = "Credenciales incorrectas" });
    }
}

public class LoginDto
{
    public string Usuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}