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

        // Inyecto DbContext y AuthService
        public UsuariosController(EcommerceDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        // 1. Registrar nuevo usuario
        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(UsuarioRegistroDto request)
        {
            // Verifico si el correo ya existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("El correo ya está registrado.");
            }

            // Creo hash de la contraseña
            _authService.CrearPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Creo el usuario
            var nuevoUsuario = new Usuario
            {
                NombreCompleto = request.NombreCompleto,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                EsAdmin = true // Temporal
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok("¡Usuario registrado con éxito!");
        }

        // 2. Login
        [HttpPost("login")]
        public async Task<ActionResult> Login(UsuarioLoginDto request)
        {
            // Busco al usuario por email
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            // Verifico la contraseña
            if (!_authService.VerificarPasswordHash(request.Password, usuario.PasswordHash, usuario.PasswordSalt))
            {
                return BadRequest("Contraseña incorrecta.");
            }

            // Genero token JWT
            var token = _authService.CrearToken(usuario);

            // Devuelvo token y mensaje
            return Ok(new
            {
                mensaje = $"¡Bienvenido {usuario.NombreCompleto}!",
                token = token
            });
        }
    }
}