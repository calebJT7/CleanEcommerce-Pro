using Infrastructure;
using Infrastructure.Repositories; // Asegúrate que tus repos estén aquí
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 1. CONFIGURACIÓN DE SERVICIOS (ZONA BUILDER)

// A. Configuración Híbrida de Base de Datos
if (builder.Environment.IsDevelopment())
{
    // MODO CASA: SQL Server
    // Usa la conexión del archivo appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<EcommerceDbContext>(options =>
        options.UseSqlServer(connectionString));
    // Registrar el servicio de encriptación
    builder.Services.AddScoped<Application.Services.AuthService>();
    //  CONFIGURACIÓN DEL GUARDIA DE SEGURIDAD (JWT)
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // Lo pongo en false para evitar rebotes por nombre de servidor
                ValidateAudience = false, // Lo pongo en false para desarrollo local
                ValidateLifetime = true,  // Mantengo la validación de fecha de expiración
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Tu_Palabra_Secreta_Super_Larga_De_32_CharsTu_Palabra_Secreta_Super_Larga_De_32_CharsTu_Palabra_Secreta_Super_Larga_De_32_Chars"))
            };
        });
}
else
{
    //  MODO NUBE (Render): PostgreSQL
    // Render nos pasará la conexión en una variable de entorno oculta
    var dbUrl = "Host=dpg-d64vbg8gjchc73feqhug-a;Database=db_ecommerce_caleb;Username=db_ecommerce_caleb_user;Password=X3dprCpnUHx8TKzfBOMBneJJPw7QThD1;Ssl Mode=Require;";
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
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. CONSTRUCCIÓN DE LA APP
var app = builder.Build();

// 3. CONFIGURACIÓN DEL PIPELINE (ZONA APP)

// A. Auto-Migración Inteligente 
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

// B. Middlewares 
app.UseMiddleware<ExceptionMiddleware>();

// C. Swagger 
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization(); // Importante agregarlo aunque no lo uses full todavía

app.MapControllers();

app.Run();