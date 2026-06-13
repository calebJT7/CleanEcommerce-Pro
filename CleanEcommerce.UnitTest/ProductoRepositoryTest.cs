using Xunit;
using Microsoft.EntityFrameworkCore; // Para la BD en memoria
using Infrastructure.Repositories;
using Infrastructure; // Para el DbContext
using Domain;
using System.Threading.Tasks;
using System.Linq;

namespace CleanEcommerce.UnitTest
{
    public class ProductoRepositoryTests
    {
        [Fact]
        public async Task AddAsync_DebeGuardarProductoEnBaseDeDatos()
        {
            // Arrange: configuro DbContext in-memory
            var options = new DbContextOptionsBuilder<EcommerceDbContext>()
                .UseInMemoryDatabase(databaseName: "BaseDeDatosTest_1") // Nombre único
                .Options;

            // Creamos el Contexto real usando esas opciones falsas
            var context = new EcommerceDbContext(options);

            // Instancio el repositorio real
            var repository = new ProductoRepository(context);

            var nuevoProducto = new Producto
            {
                Nombre = "Monitor 4K",
                Precio = 400.00m,
                Stock = 10
            };

            // Act: guardo el producto y forceo el SaveChanges
            await repository.AddAsync(nuevoProducto);
            await context.SaveChangesAsync();

            // Assert: verifico que el producto quedó en la BD
            var productoGuardado = await context.Productos.FirstOrDefaultAsync(p => p.Nombre == "Monitor 4K");

            Assert.NotNull(productoGuardado);
            Assert.Equal(400.00m, productoGuardado.Precio);
        }
    }
}