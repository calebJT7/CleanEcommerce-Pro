using Infrastructure;
using Infrastructure.Repositories; // Asegúrate que tus repos estén aquí
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERVICIOS (ZONA BUILDER)

// A. Configuración Híbrida de Base de Datos
if (builder.Environment.IsDevelopment())
{
    // MODO CASA: SQL Server
    // Usamos la conexión del archivo appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<EcommerceDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else
{
    //  MODO NUBE (Render): PostgreSQL
    // Render nos pasará la conexión en una variable de entorno oculta
    var dbUrl = "postgres://db_ecommerce_caleb_user:X3dprCpnUHx8TKzfBOMBneJJPw7QThD1@dpg-d64vbg8gjchc73feqhug-a.ohio-postgres.render.com/db_ecommerce_caleb";
    builder.Services.AddDbContext<EcommerceDbContext>(options =>
        options.UseNpgsql(dbUrl));
}

// B. Inyección de Dependencias
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductoService, ProductoService>();

// C. Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. CONSTRUCCIÓN DE LA APP
var app = builder.Build(); // <--- AQUÍ NACE LA APP 

// 3. CONFIGURACIÓN DEL PIPELINE (ZONA APP)

// A. Auto-Migración Inteligente (Tu bloque mejorado)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<EcommerceDbContext>();
        logger.LogInformation("Verificando base de datos en la nube...");

        // EnsureCreated es más seguro para Render ahora mismo que Migrate()
        // Crea la BD y las tablas si no existen.
        context.Database.EnsureCreated();

        logger.LogInformation("¡Base de datos lista!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocurrió un error al iniciar la base de datos.");
    }
}

// B. Middlewares (Tu guardián de errores)
app.UseMiddleware<ExceptionMiddleware>();

// C. Swagger (Solo en desarrollo para no exponerlo en prod, o déjalo fuera del if si quieres verlo en Render)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization(); // Importante agregarlo aunque no lo uses full todavía

app.MapControllers();

app.Run(); // <--- ¡AQUÍ CORRE LA APP! 🏃‍♂️