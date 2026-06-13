using Domain; // Importante para ver la clase Producto
using Xunit;  // La librería de testing

namespace CleanEcommerce.UnitTests
{
    public class ProductoTests
    {
        [Fact] // Marca el método como prueba
        public void CrearProducto_ConDatosValidos_DebeGuardarInformacion()
        {
            // Arrange: define valores de prueba
            string nombreEsperado = "Laptop Gamer";
            decimal precioEsperado = 1500.00m;
            int stockEsperado = 5;

            // Act: instancia el producto
            var producto = new Producto
            {
                Nombre = nombreEsperado,
                Precio = precioEsperado,
                Stock = stockEsperado
            };

            // Assert: verifica propiedades
            Assert.Equal(nombreEsperado, producto.Nombre);
            Assert.Equal(precioEsperado, producto.Precio);
            Assert.Equal(stockEsperado, producto.Stock);
        }
    }
}