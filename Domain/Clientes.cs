namespace Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public decimal DeudaTotal { get; set; } = 0;
    }
}