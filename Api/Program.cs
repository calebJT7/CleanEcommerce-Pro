using Microsoft.EntityFrameworkCore;
using Infrastructure;
// Estos usings adicionales son importantes si tienes servicios configurados
// Si te dan error, bórralos, pero por tus fotos veo que los usabas:
using Infrastructure.Repositories;
using Application.Interfaces;
using Application.Services;
// using Api.Middlewares; // Descomenta esto si usas Middlewares propios

var builder = WebApplication.CreateBuilder(args);


// 1. CONFIGURACIÓN DE SERVICIOS (ZONA BUILDER)


// A. Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// B. Base de Datos Híbrida (La lógica inteligente)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (builder.Environment.IsDevelopment())
{
    //  MODO CASA: SQL Server
    builder.Services.AddDbContext<EcommerceDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else
{
    //  MODO NUBE: PostgreSQL (Render)
    var dbUrl = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    builder.Services.AddDbContext<EcommerceDbContext>(options =>
        options.UseNpgsql(dbUrl));
}

// C. Inyección de Dependencias (Tus repositorios y servicios)
// (Asegúrate de que estas líneas coincidan con lo que tenías antes)
// builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// builder.Services.AddScoped<IProductoService, ProductoService>();



// 2. CONSTRUCCIÓN DE LA APP

var app = builder.Build(); // <--- AQUÍ NACE LA APP 👶


// 3. CONFIGURACIÓN DEL PIPELINE (ZONA APP)

// A. Auto-Migración para Render (¡Esto va AQUI, después de que app nace!)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
    // Esto crea la DB si no existe (Mágia para la nube)
    context.Database.EnsureCreated();
}

// B. Swagger y Https
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run(); // <--- AQUÍ CORRE LA APP 🏃‍♂️