namespace Application.DTOs
{
    public class PedidoDto
    {
        // ¿Quién está comprando?
        public int ClienteId { get; set; }

        // La lista de cosas que metió al carrito
        public List<DetallePedidoDto> Detalles { get; set; } = new List<DetallePedidoDto>();
    }
}