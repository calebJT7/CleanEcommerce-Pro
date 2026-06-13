namespace Web.Services
{
    public class CarritoService
    {
        //lista de cosas en el carrito
        public List<int> ProductosId { get; private set; } = new List<int>();

        //  avisa cuando algo cambia
        public event Action? OnChange;

        public void AgregarAlCarrito(int productoId)
        {
            ProductosId.Add(productoId);
            //  avisarle al menú superior
            OnChange?.Invoke();
        }

        public int ObtenerCantidad()
        {
            return ProductosId.Count;
        }
        public void VaciarCarrito()
        {
            ProductosId.Clear(); // Vaciamos la lista de la memoria
            OnChange?.Invoke();  // Tocamos la campana para que el menú vuelva a 0
        }
    }
}