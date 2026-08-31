using System.ComponentModel.DataAnnotations;

namespace Microservicio_Vehiculo.Models
{
    public class Vehiculo
    {
        [Key]
        public int IdVehiculo { get; set; }
        public int IdCategoria { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Estado { get; set; }
    }
}