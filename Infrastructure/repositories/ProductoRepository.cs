using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic; // <--- AGREGA ESTA (Para List)
using System.Threading.Tasks;     // <--- AGREGA ESTA (Para Task)

namespace Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly EcommerceDbContext _context;

        public ProductoRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetAllAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task AddAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            _context.Productos.Update(producto); // EF Core es listo y sabe actualizar
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}