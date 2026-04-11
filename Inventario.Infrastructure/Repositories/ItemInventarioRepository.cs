using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories;

public class ItemInventarioRepository : GenericRepository<ItemInventario>, IItemInventarioRepository
{
    public ItemInventarioRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<ItemInventario>> GetByUserIdAsync(string userId)
    {
        return await BuildItemDetailsQuery()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<ItemInventario?> GetByIdAndUserIdAsync(Guid id, string userId)
    {
        return await _dbSet
            .Include(x => x.Categoria)
            .Include(x => x.Local)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }

    public async Task<IEnumerable<ItemInventario>> GetByLocalAsync(Guid localId)
    {
        return await BuildItemDetailsQuery()
            .Where(x => x.LocalId == localId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemInventario>> GetItensCarosAsync(decimal valorMinimo)
    {
        return await BuildItemDetailsQuery()
            .Where(x => x.ValorCompra >= valorMinimo)
            .ToListAsync();
    }

    private IQueryable<ItemInventario> BuildItemDetailsQuery()
    {
        return _dbSet
            .AsNoTracking()
            .Include(x => x.Categoria)
            .Include(x => x.Local);
    }
}
