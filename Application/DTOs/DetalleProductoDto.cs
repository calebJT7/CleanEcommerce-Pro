namespace Application.DTOs
{
    public class DetallePedidoDto
    {
        // Solo necesitamos saber qué producto quiere y cuántos 
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}