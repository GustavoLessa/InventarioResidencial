using Inventario.Application.DTOs;

namespace Inventario.Application.Interfaces;

public interface IItemAppService
{
    Task<IEnumerable<ItemInventarioDTO>> GetAllAsync(string userId);
    Task<ItemInventarioDTO?> GetByIdAsync(Guid id, string userId);
    Task<Guid?> AddAsync(CreateItemInventarioDTO dto, string userId);
    Task<bool> UpdateAsync(Guid id, CreateItemInventarioDTO dto, string userId);
    Task<bool> DeleteAsync(Guid id, string userId);
}
