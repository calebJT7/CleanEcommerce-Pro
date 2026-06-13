namespace Domain
{
    public class Pago
    {
        public int Id { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.UtcNow;

        // Monto pagado por el cliente
        public decimal Monto { get; set; }

        // FK al cliente
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}