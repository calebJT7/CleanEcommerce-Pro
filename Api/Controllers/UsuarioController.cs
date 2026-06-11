using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure; // Para el DbContext
using Domain;         // Para Usuario
using Application.DTOs; // Para los DTOs
using Application.Services; // Para nuestro AuthService

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly EcommerceDbContext _context;
        private readonly AuthService _authService;

        // 💉 Inyectamos la base de datos y nuestro motor criptográfico
        public UsuariosController(EcommerceDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // 📝 1. REGISTRAR UN NUEVO USUARIO
        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(UsuarioRegistroDto request)
        {
            // Verificamos si el correo ya existe en la base de datos
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("El correo ya está registrado.");
            }

            // Usamos nuestro servicio para encriptar la contraseña que escribió
            _authService.CrearPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Creamos el nuevo usuario seguro
            var nuevoUsuario = new Usuario
            {
                NombreCompleto = request.NombreCompleto,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                EsAdmin = true // Por ahora hacemos a todos administradores
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok("¡Usuario registrado con éxito!");
        }

        // 🔐 2. INICIAR SESIÓN (LOGIN)
        [HttpPost("login")]
        public async Task<ActionResult> Login(UsuarioLoginDto request)
        {
            // Buscamos al usuario por su correo
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            // Verificamos si la contraseña coincide con la encriptada
            if (!_authService.VerificarPasswordHash(request.Password, usuario.PasswordHash, usuario.PasswordSalt))
            {
                return BadRequest("Contraseña incorrecta.");
            }

            // Si la contraseña es correcta, fabricamos su Pase VIP (Token)
            var token = _authService.CrearToken(usuario);

            // Le devolvemos un mensaje y su token de seguridad
            return Ok(new
            {
                mensaje = $"¡Bienvenido {usuario.NombreCompleto}!",
                token = token
            });
        }
    }
}