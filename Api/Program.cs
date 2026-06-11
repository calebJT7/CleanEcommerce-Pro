using Infrastructure;
using Infrastructure.Repositories;
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

// 1. BASE DE DATOS (Separamos Desarrollo de Producción)
if (builder.Environment.IsDevelopment())
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<EcommerceDbContext>(options => options.UseSqlServer(connectionString));
}
else
{
    var dbUrl = "Host=dpg-d64vbg8gjchc73feqhug-a;Database=db_ecommerce_caleb;Username=db_ecommerce_caleb_user;Password=X3dprCpnUHx8TKzfBOMBneJJPw7QThD1;Ssl Mode=Require;";
    builder.Services.AddDbContext<EcommerceDbContext>(options => options.UseNpgsql(dbUrl));
}

// 2. SEGURIDAD (Ahora está afuera del IF, Azure lo va a leer)
builder.Services.AddScoped<Application.Services.AuthService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Tu_Palabra_Secreta_Super_Larga_De_32_CharsTu_Palabra_Secreta_Super_Larga_De_32_CharsTu_Palabra_Secreta_Super_Larga_De_32_Chars"))
        };
    });

// 3. INYECCIÓN DE DEPENDENCIAS
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) => { cfg.Host("localhost", "/"); });
});

var app = builder.Build();

// 4. PIPELINE
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
    context.Database.EnsureCreated();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();