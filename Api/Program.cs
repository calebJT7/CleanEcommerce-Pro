using Infrastructure;
using Infrastructure.Repositories;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Api.Middlewares; // Para tu manejo de errores

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar la Base de Datos
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Inyección de Dependencias (Repositorios y Servicios)
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductoService, ProductoService>();

// 3. ACTIVAR CONTROLADORES (¡Esto es lo nuevo!)
// Le dice a .NET: "Busca clases que hereden de ControllerBase"
builder.Services.AddControllers();

// 4. Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// migracion automatoica de la bd
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<EcommerceDbContext>();
        logger.LogInformation("Intentando aplicar migraciones...");
        context.Database.Migrate(); // ¡Esto crea la BD y tablas si no existen!
        logger.LogInformation("¡Base de datos migrada correctamente!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocurrió un error al migrar la base de datos.");
    }
}

// 5. Configurar el Pipeline (Middleware)
app.UseMiddleware<ExceptionMiddleware>(); // Tu guardián de errores

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 6. MAPEAR CONTROLADORES (¡Esto reemplaza a los MapGet manuales!)
// Le dice a la app: "Usa las rutas definidas en los archivos de Controllers"
app.MapControllers();

app.Run();