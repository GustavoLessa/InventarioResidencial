using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Context;

namespace Inventario.Infrastructure.Repositories;

public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context) { }
}