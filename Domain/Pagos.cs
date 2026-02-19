namespace Domain
{
    public class Pago
    {
        public int Id { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.UtcNow;

        // ¿Cuánta plata entregó el cliente?
        public decimal Monto { get; set; }

        // 🔗 Relación: ¿Quién hizo este pago?
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}