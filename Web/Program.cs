using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web;
using Microsoft.AspNetCore.Components.Authorization;
using Web.Auth;
using Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Registramos nuestro interceptor (el peaje)
builder.Services.AddTransient<Web.Auth.AuthHeaderHandler>();

// 2. Armamos el HttpClient de forma manual conectándole el peaje
builder.Services.AddScoped(sp =>
{
    var authHandler = sp.GetRequiredService<Web.Auth.AuthHeaderHandler>();

    // Le decimos que use el motor de red estándar de Blazor por debajo
    authHandler.InnerHandler = new HttpClientHandler();

    return new HttpClient(authHandler)
    {
        BaseAddress = new Uri("https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/")
    };
});
//  Registramos el HttpClient con la API local para desarrollo
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/") });
// {
//     BaseAddress = new Uri("http://localhost:7050/api/")
// });

// Activamos la seguridad inteligente de Blazor
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
// Registramos el motor del carrito para que viva en toda la app
builder.Services.AddScoped<CarritoService>();

await builder.Build().RunAsync();
