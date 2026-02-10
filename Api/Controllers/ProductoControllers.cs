using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Infrastructure;
//using Domain.Entities; // <--- Agrego esto por si acaso la palabra "Producto" te sale en rojo

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly EcommerceDbContext _context;

        public ProductosController(IProductoService productoService, EcommerceDbContext context)
        {
            _productoService = productoService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> GetAll()
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos);
        }

        [HttpPost]
        // 👇 Ahora recibimos el DTO plano, ¡Swagger será feliz!
        public async Task<ActionResult> PostProducto(ProductoDto productoDto)
        {
            // Transformamos el DTO en la Entidad pura para la Base de Datos
            var nuevoProducto = new Producto
            {
                Nombre = productoDto.Nombre,
                //Descripcion = productoDto.Descripcion,
                Precio = productoDto.Precio,
                //Stock = productoDto.Stock
                // Nota: Si alguna de estas 4 palabras sale en rojo, 
                // bórrala o cámbiala por el nombre exacto que tenga tu ProductoDto.
            };

            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return Ok("¡Producto creado con éxito!");
        }
    }
}