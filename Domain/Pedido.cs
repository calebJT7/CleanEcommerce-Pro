namespace Domain
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // 🔗 ¡El puente! Aquí conectamos el pedido con un Cliente específico
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public decimal Total { get; set; } = 0;

        // Una lista que guardará todos los "renglones" o productos de este pedido
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}