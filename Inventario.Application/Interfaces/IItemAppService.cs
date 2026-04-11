using Inventario.Application.DTOs;

namespace Inventario.Application.Interfaces;

public interface IItemAppService
{
    Task<IEnumerable<ItemInventarioDTO>> GetAllAsync();
    Task<ItemInventarioDTO?> GetByIdAsync(Guid id);
    Task<Guid?> AddAsync(CreateItemInventarioDTO dto);
    Task<bool> UpdateAsync(Guid id, CreateItemInventarioDTO dto);
    Task<bool> DeleteAsync(Guid id);
}
