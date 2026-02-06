using System.ComponentModel.DataAnnotations;
public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Formato de Email incorrecto")]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty; // En la vida real esto se encripta, hoy no.
    public bool EsAdmin { get; set; } = false; // Para saber si puede crear productos
}