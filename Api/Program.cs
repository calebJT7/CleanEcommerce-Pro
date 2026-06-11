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
using MassTransit;

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
}
else
{
    //  MODO NUBE (Azure u otro proveedor): PostgreSQL
    var dbUrl = "Host=dpg-d64vbg8gjchc73feqhug-a;Database=db_ecommerce_caleb;Username=db_ecommerce_caleb_user;Password=X3dprCpnUHx8TKzfBOMBneJJPw7QThD1;Ssl Mode=Require;";
    builder.Services.AddDbContext<EcommerceDbContext>(options =>
        options.UseNpgsql(dbUrl));
}

// B. Seguridad y JWT (activo en cualquier ambiente)
builder.Services.AddScoped<Application.Services.AuthService>();

var jwtSecret = builder.Configuration["AppSettings:Token"];
if (string.IsNullOrWhiteSpace(jwtSecret))
{
    jwtSecret = "Tu_Palabra_Secreta_Super_Larga_De_32_CharsTu_Palabra_Secreta_Super_Larga_De_32_CharsTu_Palabra_Secreta_Super_Larga_De_32_Chars";
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

// C. Inyección de Dependencias
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductoService, ProductoService>();

// C. Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese el token JWT en el encabezado Authorization con el prefijo Bearer."
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/");
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