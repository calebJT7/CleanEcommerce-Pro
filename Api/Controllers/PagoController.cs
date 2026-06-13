using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure; // Para el EcommerceDbContext
using Domain;         // Para las Entidades
using Application.DTOs; // Para los DTOs

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public PagosController(EcommerceDbContext context)
        {
            _context = context;
        }

        // Registrar un pago
        [HttpPost]
        public async Task<ActionResult> RegistrarPago(PagoDto pagoDto)
        {
            // 1. Validaciones básicas
            if (pagoDto.Monto <= 0)
                return BadRequest("El monto del pago debe ser mayor a cero.");

            // 2. Busco al cliente que paga
            var cliente = await _context.Clientes.FindAsync(pagoDto.ClienteId);
            if (cliente == null)
                return NotFound("Error: El cliente no existe.");

            // 3. Creo el recibo
            var nuevoPago = new Pago
            {
                ClienteId = pagoDto.ClienteId,
                Monto = pagoDto.Monto,
                FechaPago = DateTime.UtcNow
            };

            // 4. Aplico regla de negocio: resto el pago de la deuda
            cliente.DeudaTotal -= pagoDto.Monto;

            // 5. Guardo en la base de datos
            _context.Pagos.Add(nuevoPago);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "¡Pago registrado con éxito!",
                montoAbonado = nuevoPago.Monto,
                deudaRestante = cliente.DeudaTotal
            });
        }

        // Obtengo historial de pagos
        [HttpGet]
        public async Task<ActionResult> GetPagos()
        {
            // Unimos los pagos con los clientes para que el dueño vea quién le pagó
            var historial = await _context.Pagos
                .Include(p => p.Cliente)
                .Select(p => new
                {
                    NumeroDeRecibo = p.Id,
                    Fecha = p.FechaPago.ToString("dd/MM/yyyy HH:mm"),
                    Cliente = p.Cliente!.NombreCompleto,
                    MontoPagado = p.Monto
                })
                .ToListAsync();

            return Ok(historial);
        }
    }
}