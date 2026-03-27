using Inventario.Domain.Entities;

namespace Inventario.Domain.Interfaces;

public interface IItemInventarioRepository : IGenericRepository<ItemInventario>
{
    // Exemplo de método específico: buscar itens de um local específico
    Task<IEnumerable<ItemInventario>> GetByLocalAsync(Guid localId);
    
    // Buscar itens que precisam de reavaliação de valor
    Task<IEnumerable<ItemInventario>> GetItensCarosAsync(decimal valorMinimo);
}