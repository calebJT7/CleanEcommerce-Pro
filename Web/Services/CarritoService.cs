namespace Web.Services
{
    public class CarritoService
    {
        // Esta es la lista de cosas en el carrito
        public List<int> ProductosId { get; private set; } = new List<int>();

        // Este evento es el "megáfono" que avisa cuando algo cambia
        public event Action? OnChange;

        public void AgregarAlCarrito(int productoId)
        {
            ProductosId.Add(productoId);
            // Tocamos la campana para avisarle al menú superior
            OnChange?.Invoke();
        }

        public int ObtenerCantidad()
        {
            return ProductosId.Count;
        }
    }
}