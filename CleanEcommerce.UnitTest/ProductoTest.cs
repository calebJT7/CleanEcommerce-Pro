using Domain; // Importante para ver la clase Producto
using Xunit;  // La librería de testing

namespace CleanEcommerce.UnitTests
{
    public class ProductoTests
    {
        [Fact] // Esta etiqueta le dice a C#: "¡Oye, esto es una prueba!"
        public void CrearProducto_ConDatosValidos_DebeGuardarInformacion()
        {
            // 1. ARRANGE (Preparar)
            // Definimos qué valores queremos probar
            string nombreEsperado = "Laptop Gamer";
            decimal precioEsperado = 1500.00m;
            int stockEsperado = 5;

            // 2. ACT (Actuar)
            // Creamos el objeto real
            var producto = new Producto
            {
                Nombre = nombreEsperado,
                Precio = precioEsperado,
                Stock = stockEsperado
            };

            // 3. ASSERT (Afirmar)
            // Preguntamos: "¿Es verdad que producto.Nombre es igual a 'Laptop Gamer'?"
            Assert.Equal(nombreEsperado, producto.Nombre);
            Assert.Equal(precioEsperado, producto.Precio);
            Assert.Equal(stockEsperado, producto.Stock);
        }
    }
}