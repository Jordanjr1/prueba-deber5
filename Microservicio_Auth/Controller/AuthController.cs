using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microservicio_Auth.Data;
using Microservicio_Auth.Models;
using Microservicio_Auth.Services;

namespace Microservicio_Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(AuthDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        // Consultar usuario en la BD AuthDB
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == dto.Usuario && u.Password == dto.Password);

        if (usuario == null)
        {
            return Unauthorized(new { mensaje = "Credenciales incorrectas" });
        }

        // Generar JWT incluyendo el nombre de usuario y su Rol
        var token = _jwtService.GenerarToken(usuario.Username, usuario.Rol);

        return Ok(new
        {
            token,
            username = usuario.Username,
            rol = usuario.Rol,
            mensaje = "Autenticación exitosa"
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var existe = await _context.Usuarios.AnyAsync(u => u.Username == dto.Usuario);
        if (existe)
        {
            return BadRequest(new { mensaje = "El usuario ya existe" });
        }

        var nuevoUsuario = new Usuario
        {
            Username = dto.Usuario,
            Password = dto.Password,
            Email = dto.Email,
            Rol = string.IsNullOrWhiteSpace(dto.Rol) ? "Normal" : dto.Rol
        };

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Usuario registrado correctamente", username = nuevoUsuario.Username, rol = nuevoUsuario.Rol });
    }
}

public class LoginDto
{
    public string Usuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterDto
{
    public string Usuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Rol { get; set; } = "Normal"; // "Admin" o "Normal"
}