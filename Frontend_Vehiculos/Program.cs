using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend_Vehiculos.Services;

namespace Frontend_Vehiculos
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Configurar HttpClient apuntando al ApiGateway en Azure
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("http://20.9.128.203:5000/")
            });

            // Registrar el servicio cliente de APIs
            builder.Services.AddScoped<ApiService>();

            await builder.Build().RunAsync();
        }
    }
}