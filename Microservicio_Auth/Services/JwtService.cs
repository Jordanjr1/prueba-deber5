using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Microservicio_Auth.Services;

public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerarToken(string usuario, string rol)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"]!;

        // 1. Crear la clave simétrica
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 2. Definir los datos guardados dentro del token (Claims)
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario),
            new Claim(ClaimTypes.Role, rol),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // 3. Fabricar el Token
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}