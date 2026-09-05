namespace Microservicio_Auth.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Rol { get; set; } = "Normal";
    }
}
