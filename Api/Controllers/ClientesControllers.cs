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

        // 🔵 OBTENER TODOS LOS CLIENTES (GET)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return Ok(clientes);
        }

        //  REGISTRAR UN NUEVO CLIENTE (POST)
        [HttpPost]
        public async Task<ActionResult> PostCliente(ClienteDto clienteDto)
        {
            // Transformamos el DTO en la Entidad real
            var nuevoCliente = new Cliente
            {
                NombreCompleto = clienteDto.NombreCompleto,
                Telefono = clienteDto.Telefono,
                DeudaTotal = 0 // ¡Todo cliente nuevo empieza con deuda cero!
            };

            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            return Ok("¡Cliente registrado con éxito!");
        }
        //  ACTUALIZAR UN CLIENTE (PUT)
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCliente(int id, ClienteDto clienteDto)
        {
            // 1. Buscamos si el cliente existe
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound("Error: cliente no existente");
            }
            // 2. Actualizamos solo los datos permitidos (¡Prohibido tocar la DeudaTotal aquí!)
            cliente.NombreCompleto = clienteDto.NombreCompleto;
            cliente.Telefono = clienteDto.Telefono;
            // 3. Guardamos los cambios
            await _context.SaveChangesAsync();
            return Ok("¡Cliente actualizado correctamente!");
        }
        //  ELIMINAR UN CLIENTE (DELETE)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            // 1. Buscamos al cliente
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound("Error: El cliente no existe.");
            }

            // 2. Lo eliminamos
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok("¡Cliente eliminado para siempre!");
        }
    }
}