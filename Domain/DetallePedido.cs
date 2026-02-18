namespace Domain
{
    public class DetallePedido
    {
        public int Id { get; set; }

        // 🔗 ¿A qué Pedido pertenece este renglón?
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }

        // 🔗 ¿Qué Producto se están llevando?
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        // ¿Cuántos se llevan y a qué precio en ese momento?
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}