namespace Inventario.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
}