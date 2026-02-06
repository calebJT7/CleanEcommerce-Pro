using Domain;
namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        IProductoRepository Productos { get; }
        Task<int> CompleteAsync();
    }
}