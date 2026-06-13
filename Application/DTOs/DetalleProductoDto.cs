namespace Application.DTOs
{
    public class DetallePedidoDto
    {
        // Solo necesita saber qué producto quiere y cuántos 
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}