namespace Domain
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // ¡Adiós string Password! Hola encriptación real 🔒
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];

        // Tu idea original conservada:
        public bool EsAdmin { get; set; } = false;
    }
}