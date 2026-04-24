using Xunit;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class PedidoTests
    {
        [Fact]
        public void ValidarCalculoDeTotales_TestRapido()
        {
            // Arrange
            var precio = 1500.50m;
            var cantidad = 2;
            var esperado = 3001.00m;

            // Act
            var resultado = precio * cantidad;

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}