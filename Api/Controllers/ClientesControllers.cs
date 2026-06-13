using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure; // Para el EcommerceDbContext
using Domain;         // Para la Entidad Cliente
using Application.DTOs; // Para el ClienteDto

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public ClientesController(EcommerceDbContext context)
        {
            _context = context;
        }

        // Obtener todos los clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return Ok(clientes);
        }

        // Registrar nuevo cliente
        [HttpPost]
        public async Task<ActionResult> PostCliente(ClienteDto clienteDto)
        {
            // Convierto DTO a entidad
            var nuevoCliente = new Cliente
            {
                NombreCompleto = clienteDto.NombreCompleto,
                Email = clienteDto.Email,
                Telefono = clienteDto.Telefono,
                DeudaTotal = 0 // Nuevo cliente inicia con deuda cero
            };

            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            return Ok("¡Cliente registrado con éxito!");
        }
        // Actualizar un cliente
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCliente(int id, ClienteDto clienteDto)
        {
            // Verifico si el cliente existe
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound("Error: cliente no existente");
            }
            // Actualizo solo los campos permitidos (no tocar DeudaTotal)
            cliente.NombreCompleto = clienteDto.NombreCompleto;
            cliente.Email = clienteDto.Email;
            cliente.Telefono = clienteDto.Telefono;
            // Guardo cambios
            await _context.SaveChangesAsync();
            return Ok("¡Cliente actualizado correctamente!");
        }
        // Eliminar un cliente
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            // Busco al cliente
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound("Error: El cliente no existe.");
            }

            // Elimino el cliente
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok("¡Cliente eliminado para siempre!");
        }
    }
}