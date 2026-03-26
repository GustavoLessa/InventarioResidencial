namespace Inventario.Domain.Entities;

public class Local : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public Local() { }

    public Local(string nome)
    {
        Nome = nome;
    }
}