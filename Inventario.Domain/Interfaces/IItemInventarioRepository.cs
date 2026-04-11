using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface IItemInventarioRepository : IGenericRepository<ItemInventario>
{
    Task<IEnumerable<ItemInventario>> GetByUserIdAsync(string userId);
    Task<ItemInventario?> GetByIdAndUserIdAsync(Guid id, string userId);
    Task<IEnumerable<ItemInventario>> GetByLocalAsync(Guid localId);
    Task<IEnumerable<ItemInventario>> GetItensCarosAsync(decimal valorMinimo);
}
