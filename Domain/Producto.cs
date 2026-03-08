using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Producto
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    // "decimal" es mejor que "double" para dinero (más preciso)
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }
    public string? Descripcion { get; set; }

    public int Stock { get; set; }
    public string ImagenUrl { get; set; } = string.Empty;
}