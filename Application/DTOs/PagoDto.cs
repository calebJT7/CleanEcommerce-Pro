namespace Application.DTOs
{
    public class PagoDto
    {
        // ¿Quién está pagando?
        public int ClienteId { get; set; }

        // ¿Cuánta plata dejó?
        public decimal Monto { get; set; }
    }
}