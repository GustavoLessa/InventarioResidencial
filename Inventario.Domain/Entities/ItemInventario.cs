namespace Inventario.Domain.Entities;

public class ItemInventario : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public decimal ValorCompra { get; set; }
    public decimal? ValorAtual { get; set; }
    public DateTime? DataAquisicao { get; set; }
    public Guid LocalId { get; set; }
    public Local? Local { get; set; }
    public Guid CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public string? ImagemUrl { get; set; }
    public string? NotaFiscalUrl { get; set; }

    public ItemInventario() { }

    public ItemInventario(string userId, string nome, decimal valorCompra, Guid categoriaId, Guid localId)
    {
        UserId = userId;
        Nome = nome;
        ValorCompra = valorCompra;
        CategoriaId = categoriaId;
        LocalId = localId;
    }
}
