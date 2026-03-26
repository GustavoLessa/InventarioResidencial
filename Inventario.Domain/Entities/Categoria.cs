namespace Inventario.Domain.Entities;

public class Categoria : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    // Construtor vazio para o ORM
    public Categoria() { }

    public Categoria(string nome, string? descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}