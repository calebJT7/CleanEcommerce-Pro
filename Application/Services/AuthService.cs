using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain; // Para conocer a la clase Usuario
using Microsoft.Extensions.Configuration; // Para leer el appsettings.json
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;

        // Inyecto la configuración para leer el secreto del appsettings.json
        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        // 1. Creo hash de contraseña (registro)
        public void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // 2. Verifico la contraseña (login)
        public bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // 3. Genero JWT
        public string CrearToken(Usuario usuario)
        {
            // Claims del token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            // Compruebo si el email es admin (ignorando mayúsculas)
            if (usuario.Email.Equals("bangtankpos375@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                // Uso un claim de rol estándar
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                // Fallback de seguridad
                claims.Add(new Claim(ClaimTypes.Role, "Cliente"));
            }

            // Obtengo la clave secreta
            var tokenKey = _config.GetSection("AppSettings:Token").Value;
            if (string.IsNullOrWhiteSpace(tokenKey))
            {
                tokenKey = "Tu_Palabra_Secreta_Super_Larga_De_32_CharsTu_Palabra_Secreta_Super_Larga_De_32_CharsTu_Palabra_Secreta_Super_Larga_De_32_Chars";
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            // Creo las credenciales de firma
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Configuro expiración (1 día)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            // Genero el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}