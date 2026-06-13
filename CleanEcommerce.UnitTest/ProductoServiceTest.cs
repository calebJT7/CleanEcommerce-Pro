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
            // Arrange: prepara datos y mocks

            // Creo lista falsa de productos (simula la BD)
            var listaFalsa = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Mouse", Precio = 10, Stock = 5 },
                new Producto { Id = 2, Nombre = "Teclado", Precio = 20, Stock = 3 }
            };

            // Creo mock del repositorio
            var mockRepo = new Mock<IProductoRepository>();
            // Configuro GetAllAsync para que devuelva la lista falsa
            mockRepo.Setup(repo => repo.GetAllAsync())
                    .ReturnsAsync(listaFalsa);

            // Creo mock del UnitOfWork
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            // Le decimos: "Cuando te pidan la propiedad Productos, devuelve al bibliotecario falso"
            mockUnitOfWork.Setup(u => u.Productos).Returns(mockRepo.Object);

            // Instancio el servicio con los mocks
            var servicio = new ProductoService(mockUnitOfWork.Object);

            // Act: llamo al servicio
            var resultado = await servicio.GetAllAsync();

            // Assert: verifico resultados esperados
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Mouse", resultado[0].Nombre);
        }
    }
}