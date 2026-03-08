namespace Application.DTOs
{
    public class PedidoAdminDto
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ClienteEmail { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}