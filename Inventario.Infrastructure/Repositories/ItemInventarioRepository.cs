using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Repositories
{
    public class ItemInventarioRepository : GenericRepository<ItemInventario>, IItemInventarioRepository
    {
        public ItemInventarioRepository(AppDbContext context) : base(context) { }
        public async Task<IEnumerable<ItemInventario>> GetByLocalAsync(Guid localId)
        {
            return await _dbSet.Where(x => x.LocalId == localId).ToListAsync();
        }

        public async Task<IEnumerable<ItemInventario>> GetItensCarosAsync(decimal valorMinimo)
        {
            return await _dbSet.Where(x => x.ValorCompra >= valorMinimo).ToListAsync();
        }
    }
       
}