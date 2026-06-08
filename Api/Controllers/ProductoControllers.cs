using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Domain;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public ProductosController(EcommerceDbContext context)
        {
            _context = context;
        }

        //  1. LEER TODOS
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> GetProductos()
        {
            var productos = await _context.Productos
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    Descripcion = p.Descripcion,
                    ImagenUrl = p.ImagenUrl ?? "" //  NUEVO: Mandamos la foto al frontend
                })
                .ToListAsync();

            return Ok(productos);
        }

        //  2. LEER UNO SOLO
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null) return NotFound("El producto no existe.");

            return Ok(new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Descripcion = producto.Descripcion,
                ImagenUrl = producto.ImagenUrl ?? "" //  NUEVO: Mandamos la foto al frontend
            });
        }

        //  3. CREAR NUEVO
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CrearProducto(ProductoDto productoDto)
        {
            var nuevoProducto = new Producto
            {
                Nombre = productoDto.Nombre,
                Precio = productoDto.Precio,
                Stock = productoDto.Stock,
                Descripcion = productoDto.Descripcion,
                ImagenUrl = productoDto.ImagenUrl //  NUEVO: Guardamos la foto en la BD
            };

            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Producto creado" });
        }

        //  4. ACTUALIZAR
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarProducto(int id, ProductoDto productoDto)
        {
            var productoDB = await _context.Productos.FindAsync(id);
            if (productoDB == null) return NotFound();

            productoDB.Nombre = productoDto.Nombre;
            productoDB.Precio = productoDto.Precio;
            productoDB.Stock = productoDto.Stock;
            productoDB.Descripcion = productoDto.Descripcion;
            productoDB.ImagenUrl = productoDto.ImagenUrl; //  NUEVO: Actualizamos la foto en la BD

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Producto actualizado" });
        }

        //  5. BORRAR
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarProducto(int id)
        {
            var productoDB = await _context.Productos.FindAsync(id);
            if (productoDB == null) return NotFound();

            _context.Productos.Remove(productoDB);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Producto eliminado" });
        }
    }
}