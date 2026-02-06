namespace Application.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        //  NO inclui el Stock.
        // Quizás por estrategia de negocio no quiero que sepan cuánto stock exacto hay.
    }
}