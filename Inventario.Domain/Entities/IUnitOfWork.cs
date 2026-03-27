namespace Inventario.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IItemInventarioRepository Itens { get; }
    ILocalRepository Locais { get; }
    ICategoriaRepository Categorias { get; }
    
    Task<int> CommitAsync(); // Onde o "SaveChanges" do Banco acontece
}