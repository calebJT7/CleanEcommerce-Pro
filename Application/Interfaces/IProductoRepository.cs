using Domain;
namespace Application.Interfaces
{
    public interface IProductoRepository
    {
        // Solo definir las firmas (lo que entra y lo que sale)
        Task<List<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(int id);
        Task AddAsync(Producto producto);
        Task UpdateAsync(Producto producto);
        Task DeleteAsync(int id);
    }
}