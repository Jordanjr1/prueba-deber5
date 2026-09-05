using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservicio_Auth.Models;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Rol { get; set; } = "Normal";
}