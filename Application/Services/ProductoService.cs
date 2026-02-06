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
            // 1. Traemos los datos crudos de la Base de Datos (Entidades)
            var productosEntidad = await _unitOfWork.Productos.GetAllAsync();

            // 2. MAPPING MANUAL: Convertimos Entidad -> DTO
            var listaDtos = new List<ProductoDto>();

            foreach (var producto in productosEntidad)
            {
                listaDtos.Add(new ProductoDto
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio
                    // ¡Fíjate que ignoramos el Stock a propósito!
                });
            }

            return listaDtos;
        }

        public async Task<ProductoDto> CreateAsync(CreateProductoDto dto)
        {
            // 1. MAPPING MANUAL: Convertimos DTO -> Entidad
            var nuevoProducto = new Producto
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Stock = dto.Stock
            };

            // 2. Guardamos en base de datos usando el Repo
            await _unitOfWork.Productos.AddAsync(nuevoProducto);
            await _unitOfWork.CompleteAsync(); // ¡Guardar cambios!

            // 3. Devolvemos el DTO resultante (con el ID que generó la BD)
            return new ProductoDto
            {
                Id = nuevoProducto.Id,
                Nombre = nuevoProducto.Nombre,
                Precio = nuevoProducto.Precio
            };
        }
    }
}










