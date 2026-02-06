using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;

namespace Api.Controllers // <--- ¡Aquí está la clave!
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        // Inyectamos el servicio (igual que hicimos en Program.cs)
        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet] // Este método responde a GET /api/productos
        public async Task<ActionResult<List<ProductoDto>>> GetAll()
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos); // Devuelve HTTP 200 OK con los datos
        }
    }
}