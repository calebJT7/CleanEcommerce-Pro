using System.ComponentModel.DataAnnotations; // Para las validaciones

namespace Application.DTOs
{
    public class CreateProductoDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Range(0.01, 10000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        public int Stock { get; set; }
    }
}