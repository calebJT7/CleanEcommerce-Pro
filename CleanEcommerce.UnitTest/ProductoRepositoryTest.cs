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
            // 1. ARRANGE (Preparar la BD en Memoria)
            // Creamos opciones para decir: "Usa InMemory, no SQL Server real"
            var options = new DbContextOptionsBuilder<EcommerceDbContext>()
                .UseInMemoryDatabase(databaseName: "BaseDeDatosTest_1") // Nombre único
                .Options;

            // Creamos el Contexto real usando esas opciones falsas
            var context = new EcommerceDbContext(options);

            // Creamos el Repositorio REAL (nada de Mocks hoy)
            var repository = new ProductoRepository(context);

            var nuevoProducto = new Producto
            {
                Nombre = "Monitor 4K",
                Precio = 400.00m,
                Stock = 10
            };

            // 2. ACT (Guardar de verdad)
            await repository.AddAsync(nuevoProducto);
            // IMPORTANTE: En el test manual, el UnitOfWork hace el SaveChanges.
            // Aquí, como probamos solo el repo, forzamos el guardado para verificar.
            await context.SaveChangesAsync();

            // 3. ASSERT (Verificar consultando la BD)
            // Buscamos en la BD si existe el producto
            var productoGuardado = await context.Productos.FirstOrDefaultAsync(p => p.Nombre == "Monitor 4K");

            Assert.NotNull(productoGuardado); // ¿Existe?
            Assert.Equal(400.00m, productoGuardado.Precio); // ¿El precio está bien?
        }
    }
}