using Application.Interfaces;       // <--- Para ver IUnitOfWork
using System.Threading.Tasks;       // <--- Para ver Task
using Infrastructure;               // Para ver EcommerceDbContext

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EcommerceDbContext _context;

        public IProductoRepository Productos { get; private set; }

        public UnitOfWork(EcommerceDbContext context)
        {
            _context = context;
            Productos = new ProductoRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}