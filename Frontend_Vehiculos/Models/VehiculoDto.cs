namespace Frontend_Vehiculos.Models
{
    public class VehiculoDto
    {
        public int IdVehiculo { get; set; }
        public int IdCategoria { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Estado { get; set; } = true; // <-- Cambiado de int a bool
    }
}