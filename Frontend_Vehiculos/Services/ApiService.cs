using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Frontend_Vehiculos.Models;

namespace Frontend_Vehiculos.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        public string Token { get; private set; } = string.Empty;

        public ApiService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> LoginAsync(string usuario, string password)
        {
            var loginData = new { Usuario = usuario, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    Token = result.Token;
                    SetAuthHeader();
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", Token);
                    return true;
                }
            }
            return false;
        }

        public async Task CargarTokenGuardadoAsync()
        {
            var tokenGuardado = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrEmpty(tokenGuardado))
            {
                Token = tokenGuardado;
                SetAuthHeader();
            }
        }

        public async Task LogoutAsync()
        {
            Token = string.Empty;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }

        // ==================== CATEGORÍAS ====================
        public async Task<List<CategoriaDto>?> GetCategoriasAsync()
        {
            SetAuthHeader();
            return await _httpClient.GetFromJsonAsync<List<CategoriaDto>>("api/Categorias");
        }

        public async Task<bool> CreateCategoriaAsync(CategoriaDto categoria)
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync("api/Categorias", categoria);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoriaAsync(CategoriaDto categoria)
        {
            SetAuthHeader();
            var response = await _httpClient.PutAsJsonAsync($"api/Categorias/{categoria.IdCategoria}", categoria);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoriaAsync(int id)
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync($"api/Categorias/{id}");
            return response.IsSuccessStatusCode;
        }

        // ==================== VEHÍCULOS ====================
        public async Task<List<VehiculoDto>?> GetVehiculosAsync()
        {
            SetAuthHeader();
            return await _httpClient.GetFromJsonAsync<List<VehiculoDto>>("api/Vehiculos");
        }

        public async Task<bool> CreateVehiculoAsync(VehiculoDto vehiculo)
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync("api/Vehiculos", vehiculo);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> RegisterAsync(string usuario, string password, string email, string rol = "Normal")
        {
            var registerData = new { Usuario = usuario, Password = password, Email = email, Rol = rol };
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registerData);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateVehiculoAsync(VehiculoDto vehiculo)
        {
            SetAuthHeader();
            var response = await _httpClient.PutAsJsonAsync($"api/Vehiculos/{vehiculo.IdVehiculo}", vehiculo);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Status: {response.StatusCode}, Detalle: {error}");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteVehiculoAsync(int id)
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync($"api/Vehiculos/{id}");
            return response.IsSuccessStatusCode;
        }

        private void SetAuthHeader()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);
            }
        }
    }


    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}