using Inventario.Domain.Interfaces;
using Inventario.Infrastructure.Context;

namespace Inventario.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IItemInventarioRepository Itens { get; private set; }
    public ILocalRepository Locais { get; private set; }
    public ICategoriaRepository Categorias { get; private set; }

    public UnitOfWork(AppDbContext context, 
                      IItemInventarioRepository itens, 
                      ILocalRepository locais, 
                      ICategoriaRepository categorias)
    {
        _context = context;
        Itens = itens;
        Locais = locais;
        Categorias = categorias;
    }

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}