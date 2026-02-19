using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure; // Para el EcommerceDbContext
using Domain;         // Para las Entidades
using Application.DTOs; // Para los DTOs
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class PedidosController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public PedidosController(EcommerceDbContext context)
        {
            _context = context;
        }
        // 🔵 OBTENER HISTORIAL DE VENTAS (GET)
        [HttpGet]
        public async Task<ActionResult> GetPedidos()
        {
            // Magia Relacional: Unimos las 4 tablas (Pedidos, Clientes, Detalles, Productos)
            var historial = await _context.Pedidos
                .Include(p => p.Cliente)                   // Traemos los datos del Cliente
                .Include(p => p.Detalles)                  // Traemos los Renglones
                    .ThenInclude(d => d.Producto)          // Y de cada renglón, traemos el Producto
                .Select(p => new
                {
                    // Diseñamos el "Ticket" a medida para que se vea hermoso en el Frontend
                    NumeroDeFactura = p.Id,
                    Fecha = p.FechaCreacion.ToString("dd/MM/yyyy HH:mm"),
                    Cliente = p.Cliente!.NombreCompleto,
                    TotalFactura = p.Total,
                    DetalleDeCompra = p.Detalles.Select(d => new
                    {
                        Producto = d.Producto!.Nombre,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Subtotal = d.Cantidad * d.PrecioUnitario
                    })
                })
                .ToListAsync();

            return Ok(historial);
        }

        // 🛒 REGISTRAR UNA NUEVA VENTA (POST)
        [HttpPost]
        public async Task<ActionResult> CrearPedido(PedidoDto pedidoDto)
        {
            // 1. Verificamos que el cliente exista
            var cliente = await _context.Clientes.FindAsync(pedidoDto.ClienteId);
            if (cliente == null)
                return NotFound("Error: El cliente no existe.");

            // 2. Preparamos el "Ticket" o cabecera del pedido
            var nuevoPedido = new Pedido
            {
                ClienteId = pedidoDto.ClienteId,
                FechaCreacion = DateTime.UtcNow,
                Total = 0, // Empezamos en cero, ahora lo calculamos
                Detalles = new List<DetallePedido>()
            };

            // 3. Revisamos el "Carrito" (los productos que nos enviaron)
            foreach (var item in pedidoDto.Detalles)
            {
                // Buscamos el producto en la base de datos
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto == null)
                    return NotFound($"Error: El producto con ID {item.ProductoId} no existe.");

                // ¡REGLA DE NEGOCIO!: ¿Hay stock?
                if (producto.Stock < item.Cantidad)
                    return BadRequest($"No hay stock suficiente de '{producto.Nombre}'. Stock actual: {producto.Stock}");

                // Si todo está bien, RESTAMOS el stock
                producto.Stock -= item.Cantidad;

                // Sumamos el dinero al total del ticket
                var subtotal = producto.Precio * item.Cantidad;
                nuevoPedido.Total += subtotal;

                // Agregamos el "Renglón" a la factura
                nuevoPedido.Detalles.Add(new DetallePedido
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.Precio // Guardamos el precio histórico
                });
            }

            // 4. ¡REGLA DE NEGOCIO!: Le sumamos la deuda al cliente
            cliente.DeudaTotal += nuevoPedido.Total;

            // 5. Guardamos TODO junto en la base de datos
            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "¡Venta registrada con éxito!",
                totalAPagar = nuevoPedido.Total,
                nuevaDeudaCliente = cliente.DeudaTotal
            });
        }
    }
}