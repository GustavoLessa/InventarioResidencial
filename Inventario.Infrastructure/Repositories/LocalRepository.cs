using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Context;

namespace Inventario.Infrastructure.Repositories;

public class LocalRepository : GenericRepository<Local>, ILocalRepository
{
    public LocalRepository(AppDbContext context) : base(context) { }
}