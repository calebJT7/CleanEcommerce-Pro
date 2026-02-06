using Xunit;
using Moq;
using Api.Controllers; // Necesario para ver el Controller
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc; // Necesario para los resultados HTTP (Ok, BadRequest)
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanEcommerce.UnitTest
{
    public class ProductoControllerTests
    {
        [Fact]
        public async Task GetAll_DebeRetornarStatus200OK_ConListaDeProductos()
        {
            // 1. ARRANGE (Preparar)
            // Creamos la lista falsa de DTOs que devolvería el servicio
            var listaFalsa = new List<ProductoDto>
            {
                new ProductoDto { Id = 1, Nombre = "Auriculares", Precio = 50 }
            };

            // Mockeamos el SERVICIO (El controlador habla con el servicio, no con la BD)
            var mockService = new Mock<IProductoService>();
            mockService.Setup(s => s.GetAllAsync())
                       .ReturnsAsync(listaFalsa);

            // Creamos el Controlador REAL e inyectamos el servicio FALSO
            var controller = new ProductosController(mockService.Object);

            // 2. ACT (Actuar)
            // Llamamos al método como si fueramos una petición HTTP
            var resultado = await controller.GetAll();

            // 3. ASSERT (Verificar)
            // Verificamos que la respuesta sea de tipo "OkObjectResult" (Código 200)
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);

            // Verificamos que dentro del 200 venga nuestra lista
            var listaRetornada = Assert.IsType<List<ProductoDto>>(okResult.Value);
            Assert.Single(listaRetornada); // Que tenga 1 elemento
        }
    }
}