using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web;
using Microsoft.AspNetCore.Components.Authorization;
using Web.Auth;
using Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Registro interceptor
builder.Services.AddTransient<Web.Auth.AuthHeaderHandler>();

// 2. Configuro HttpClient con mi handler
builder.Services.AddScoped(sp =>
{
    var authHandler = sp.GetRequiredService<Web.Auth.AuthHeaderHandler>();

    // Uso HttpClientHandler interno
    authHandler.InnerHandler = new HttpClientHandler();

    return new HttpClient(authHandler)
    {
        BaseAddress = new Uri("https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/")
    };
});
// Alternativa: registrar HttpClient para API local (dev)
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api-caleb-ecommerce-fzbqhjhhhufzcybp.centralus-01.azurewebsites.net/") });
// {
//     BaseAddress = new Uri("http://localhost:7050/api/")
// });

// Habilito autorización
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
// Registro servicio del carrito
builder.Services.AddScoped<CarritoService>();

await builder.Build().RunAsync();
