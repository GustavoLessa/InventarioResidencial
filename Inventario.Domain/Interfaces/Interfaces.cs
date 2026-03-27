using System.Linq.Expressions;

namespace Inventario.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    
    // Método para buscas personalizadas (ex: buscar por nome)
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}