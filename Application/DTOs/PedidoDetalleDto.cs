namespace Application.DTOs
{
    public class PedidoDetalleDto
    {
        public int Id { get; set; }
        public string Fecha { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal TotalFinal { get; set; }

        // La lista de cosas que compró
        public List<RenglonDto> Renglones { get; set; } = new List<RenglonDto>();
    }

    public class RenglonDto
    {
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}