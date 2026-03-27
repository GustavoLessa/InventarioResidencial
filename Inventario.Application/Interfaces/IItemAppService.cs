using Inventario.Application.DTOs;

namespace Inventario.Application.Interfaces;

public interface IItemAppService
{
    Task<IEnumerable<ItemInventarioDTO>> GetAllAsync();
    Task<ItemInventarioDTO?> GetByIdAsync(Guid id);
    Task<bool> AddAsync(CreateItemInventarioDTO dto);
}