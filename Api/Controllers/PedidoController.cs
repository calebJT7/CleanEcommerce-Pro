using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure; // Para el EcommerceDbContext
using Domain;         // Para las Entidades
using Application.DTOs; // Para los DTOs
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        // 📋 1. OBTENER TODOS LOS PEDIDOS (Para el Panel Admin en Blazor)
        // Ruta: GET /api/pedidos
        [HttpGet]
        public async Task<ActionResult<List<PedidoAdminDto>>> GetTodosLosPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.FechaCreacion)
                .Select(p => new PedidoAdminDto
                {
                    Id = p.Id,
                    FechaCreacion = p.FechaCreacion,
                    ClienteEmail = p.Cliente!.NombreCompleto, // Usamos el nombre completo porque no hay columna de email
                    Total = p.Total,
                    Estado = p.Estado ?? "Pendiente" // Valor por defecto en caso de que esté vacío
                })
                .ToListAsync();

            return Ok(pedidos);
        }
        // 🔍 2. OBTENER HISTORIAL DETALLADO (Formato Ticket)
        // Ruta: GET /api/pedidos/historial
        [HttpGet("historial")]
        public async Task<ActionResult> GetPedidosDetallados()
        {
            var historial = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Select(p => new
                {
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

        // 🛒 3. REGISTRAR UNA NUEVA VENTA (Checkout)
        // Ruta: POST /api/pedidos
        [HttpPost]
        public async Task<ActionResult> CrearPedido([FromBody] CrearPedidoDto peticion)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized("No se pudo identificar al usuario desde el token.");

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.NombreCompleto == userEmail);

            if (cliente == null)
                return NotFound($"Error: El cliente con email {userEmail} no existe en la base de datos.");

            var nuevoPedido = new Pedido
            {
                ClienteId = cliente.Id,
                FechaCreacion = DateTime.UtcNow,
                Total = 0,
                Detalles = new List<DetallePedido>()
            };

            foreach (var pId in peticion.ProductoIds)
            {
                var producto = await _context.Productos.FindAsync(pId);
                if (producto == null) continue;

                // Actualización de inventario y totales
                producto.Stock -= 1;
                nuevoPedido.Total += producto.Precio;
                nuevoPedido.Detalles.Add(new DetallePedido
                {
                    ProductoId = pId,
                    Cantidad = 1,
                    PrecioUnitario = producto.Precio
                });
            }

            cliente.DeudaTotal += nuevoPedido.Total;
            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Venta registrada con éxito!" });
        }

        // 🚚 4. ACTUALIZAR ESTADO DEL PEDIDO
        // Ruta: PUT /api/pedidos/{id}/estado
        [HttpPut("{id}/estado")]
        public async Task<ActionResult> ActualizarEstado(int id, [FromBody] CambiarEstadoDto peticion)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound("El pedido no existe.");

            // Actualizamos el estado con la palabra que nos mande el Frontend
            pedido.Estado = peticion.NuevoEstado;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = $"El pedido #{id} ahora está: {peticion.NuevoEstado}" });
        }

        // 👁️ 5. VER EL DETALLE EXACTO DE UN PEDIDO
        // Ruta: GET /api/pedidos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Application.DTOs.PedidoDetalleDto>> GetPedidoDetalle(int id)
        {
            // Buscamos el pedido con toda su familia (Cliente, Detalles y los Productos de esos detalles)
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound("El pedido no existe.");

            // Armamos el ticket
            var ticket = new Application.DTOs.PedidoDetalleDto
            {
                Id = pedido.Id,
                Fecha = pedido.FechaCreacion.ToString("dd/MM/yyyy HH:mm"),
                Cliente = pedido.Cliente!.NombreCompleto,
                Estado = pedido.Estado ?? "Pendiente",
                TotalFinal = pedido.Total,
                Renglones = pedido.Detalles.Select(d => new Application.DTOs.RenglonDto
                {
                    Producto = d.Producto!.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Cantidad * d.PrecioUnitario
                }).ToList()
            };

            return Ok(ticket);
        }
        // 🛍️ 6. VER MIS COMPRAS (Exclusivo para el cliente)
        // Ruta: GET /api/pedidos/mis-compras
        [HttpGet("mis-compras")]
        public async Task<ActionResult<List<PedidoAdminDto>>> GetMisPedidos()
        {
            // 1. Identificamos quién es el usuario leyendo su Token VIP
            var userEmail = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized("No se pudo identificar al usuario.");

            // 2. Buscamos en la base de datos SOLO los pedidos que coincidan con su nombre/email
            var misPedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Where(p => p.Cliente!.NombreCompleto == userEmail)
                .OrderByDescending(p => p.FechaCreacion)
                .Select(p => new PedidoAdminDto
                {
                    Id = p.Id,
                    FechaCreacion = p.FechaCreacion,
                    ClienteEmail = p.Cliente!.NombreCompleto,
                    Total = p.Total,
                    Estado = p.Estado ?? "Pendiente"
                })
                .ToListAsync();

            return Ok(misPedidos);
        }
        // 📊 7. ESTADÍSTICAS DEL DASHBOARD (Para el Inicio del Admin)
        // Ruta: GET /api/pedidos/estadisticas
        [HttpGet("estadisticas")]
        public async Task<ActionResult<EstadisticasDto>> GetEstadisticas()
        {
            var pedidos = await _context.Pedidos.ToListAsync();

            var stats = new EstadisticasDto
            {
                TotalIngresos = pedidos.Sum(p => p.Total),
                CantidadPedidos = pedidos.Count,
                PedidosPendientes = pedidos.Count(p => p.Estado == "Pendiente" || string.IsNullOrEmpty(p.Estado))
            };

            return Ok(stats);
        }
    }

    //  DTO para recibir la compra desde el Frontend
    public class CrearPedidoDto
    {
        public List<int> ProductoIds { get; set; } = new List<int>();
    }
    public class CambiarEstadoDto
    {
        public string NuevoEstado { get; set; } = string.Empty;
    }
}