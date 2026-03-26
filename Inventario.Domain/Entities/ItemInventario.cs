namespace Inventario.Domain.Entities;

public class ItemInventario : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    
    // Valores financeiros
    public decimal ValorCompra { get; set; }
    public decimal? ValorAtual { get; set; }
    public DateTime? DataAquisicao { get; set; }
    
    // Relacionamento com Local
    public Guid LocalId { get; set; }
    public virtual Local? Local { get; set; }

    // Relacionamento com Categoria
    public Guid CategoriaId { get; set; }
    public virtual Categoria? Categoria { get; set; }

    // Uploads (Armazenaremos as URLs do Supabase Storage)
    public string? ImagemUrl { get; set; }
    public string? NotaFiscalUrl { get; set; } // Nova propriedade para o documento

    public ItemInventario() { }

    public ItemInventario(string nome, decimal valorCompra, Guid categoriaId, Guid localId)
    {
        Nome = nome;
        ValorCompra = valorCompra;
        CategoriaId = categoriaId;
        LocalId = localId;
    }
}