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

        //  Inyectamos la configuración para poder leer el secreto del appsettings.json
        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        //  1. MÉTODO PARA ENCRIPTAR (REGISTRO)
        public void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        //  2. MÉTODO PARA VERIFICAR (LOGIN)
        public bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // 🎫 3. MÉTODO PARA CREAR EL PASE VIP (JWT)
        public string CrearToken(Usuario usuario)
        {
            // A. ¿Qué información llevará el pase? (Claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            // 🔥 El arreglo: Ahora leemos la propiedad de la base de datos
            // Hardcodeamos TU mail de prueba actual como el jefe supremo
            if (usuario.Email == "bangtankpos375@gmail.com")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim("role", "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Cliente"));
                claims.Add(new Claim("role", "Cliente"));
            }

            // B. Buscamos la firma secreta del servidor
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value!));

            // C. Creamos el sello de seguridad
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // D. Diseñamos el Token (Dura 1 día exacto)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            // E. Fabricamos el texto final
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}