using Application.DTOs;
using Application.Interfaces;
using Domain;

namespace Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductoDto>> GetAllAsync()
        {
            // Obtengo entidades desde la BD
            var productosEntidad = await _unitOfWork.Productos.GetAllAsync();

            // Convierto Entidad -> DTO
            var listaDtos = new List<ProductoDto>();

            foreach (var producto in productosEntidad)
            {
                listaDtos.Add(new ProductoDto
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio
                    // Ignoro el Stock intencionalmente
                });
            }

            return listaDtos;
        }

        public async Task<ProductoDto> CreateAsync(CreateProductoDto dto)
        {
            // Convierto DTO -> Entidad
            var nuevoProducto = new Producto
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Stock = dto.Stock
            };

            // Guardo en la BD usando el repositorio
            await _unitOfWork.Productos.AddAsync(nuevoProducto);
            await _unitOfWork.CompleteAsync(); // Confirmo cambios

            // Devuelvo DTO con el ID generado
            return new ProductoDto
            {
                Id = nuevoProducto.Id,
                Nombre = nuevoProducto.Nombre,
                Precio = nuevoProducto.Precio
            };
        }
    }
}










