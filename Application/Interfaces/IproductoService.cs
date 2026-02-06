using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProductoService
    {
        // Devuelve una lista de DTOs, no de Entidades
        Task<List<ProductoDto>> GetAllAsync();

        // Recibe un DTO de creación, devuelve el DTO creado
        Task<ProductoDto> CreateAsync(CreateProductoDto dto);
    }
}