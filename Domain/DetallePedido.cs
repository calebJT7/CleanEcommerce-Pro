namespace Domain
{
    public class DetallePedido
    {
        public int Id { get; set; }

        // FK al pedido
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }

        // FK al producto
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        // Cantidad y precio unitario en el momento de la compra
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}