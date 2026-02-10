using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Infrastructure;

namespace Api.Controllers // <--- ¡Aquí está la clave!
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        [Route("api/[controller]")] // O como lo tengas
        [ApiController]
        public class ProductoController : ControllerBase
        {
            private readonly IProductoService _productoService;
            private readonly EcommerceDbContext _context; // <--- 1. AGREGAMOS ESTA VARIABLE

            // 2. MODIFICAMOS EL CONSTRUCTOR PARA RECIBIR AMBOS
            public ProductoController(IProductoService productoService, EcommerceDbContext context)
            {
                _productoService = productoService;
                _context = context; // <--- 3. GUARDAMOS EL CONTEXTO AQUÍ
            }

            [HttpGet] // Este método responde a GET /api/productos
            public async Task<ActionResult<List<ProductoDto>>> GetAll()
            {
                var productos = await _productoService.GetAllAsync();
                return Ok(productos); // Devuelve HTTP 200 OK con los datos
            }
            [HttpPost] // <--- Esto le dice a Swagger: "¡Ponme un botón verde!"
            public async Task<ActionResult<Producto>> PostProducto(Producto producto)
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                return Ok(producto);
            }
        }
    }
}