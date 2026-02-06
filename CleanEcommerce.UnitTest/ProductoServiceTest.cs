using Xunit;
using Moq; // La herramienta de mentiras
using Application.Services;
using Application.Interfaces;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanEcommerce.UnitTest
{
    public class ProductoServiceTests
    {
        [Fact]
        public async Task GetAllAsync_DebeRetornarListaDeDtos()
        {
            // 1. ARRANGE (Preparar el escenario falso)

            // A. Creamos una lista falsa de productos (lo que devolvería la BD)
            var listaFalsa = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Mouse", Precio = 10, Stock = 5 },
                new Producto { Id = 2, Nombre = "Teclado", Precio = 20, Stock = 3 }
            };

            // B. Creamos el Mock del REPOSITORIO (El bibliotecario falso)
            var mockRepo = new Mock<IProductoRepository>();
            // Le enseñamos a mentir: "Cuando te pidan GetAllAsync, devuelve la listaFalsa"
            mockRepo.Setup(repo => repo.GetAllAsync())
                    .ReturnsAsync(listaFalsa);

            // C. Creamos el Mock del UNIT OF WORK (El jefe falso)
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            // Le decimos: "Cuando te pidan la propiedad Productos, devuelve al bibliotecario falso"
            mockUnitOfWork.Setup(u => u.Productos).Returns(mockRepo.Object);

            // D. Instanciamos el SERVICIO REAL, pero le inyectamos el jefe FALSO
            var servicio = new ProductoService(mockUnitOfWork.Object);

            // 2. ACT (Ejecutar)
            var resultado = await servicio.GetAllAsync();

            // 3. ASSERT (Verificar)
            Assert.NotNull(resultado); // Que no devuelva nulo
            Assert.Equal(2, resultado.Count); // Que devuelva 2 elementos
            Assert.Equal("Mouse", resultado[0].Nombre); // Que el primero sea el Mouse
        }
    }
}