using Xunit;
using Api.Controllers;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanEcommerce.UnitTest
{
    public class ProductoControllerTests
    {
        [Fact]
        public async Task GetProductos_DebeRetornarStatus200OK_ConListaDeProductos()
        {
            // 1. ARRANGE (Preparar)
            // Creamos un contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<EcommerceDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Creamos datos de prueba
            using (var context = new EcommerceDbContext(options))
            {
                context.Productos.Add(new Producto
                {
                    Id = 1,
                    Nombre = "Auriculares",
                    Precio = 50,
                    Stock = 10,
                    Descripcion = "Test product"
                });
                await context.SaveChangesAsync();
            }

            // Creamos el controlador con el contexto de prueba
            using (var context = new EcommerceDbContext(options))
            {
                var controller = new ProductosController(context);

                // 2. ACT (Actuar)
                var resultado = await controller.GetProductos();

                // 3. ASSERT (Verificar)
                var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
                var listaRetornada = Assert.IsType<List<ProductoDto>>(okResult.Value);
                Assert.Single(listaRetornada);
                Assert.Equal("Auriculares", listaRetornada[0].Nombre);
                Assert.Equal(50, listaRetornada[0].Precio);
            }
        }
    }
}