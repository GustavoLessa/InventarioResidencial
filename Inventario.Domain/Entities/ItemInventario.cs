namespace Inventario.Domain.Entities;

public class ItemInventario : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string Localizacao { get; set; } = string.Empty; // Ex: Sala, Garagem
    
    // Valores financeiros
    public decimal ValorCompra { get; set; }
    public decimal? ValorAtual { get; set; }
    
    public DateTime? DataAquisicao { get; set; }
    
    // Relacionamento com Categoria
    public Guid CategoriaId { get; set; }
    public virtual Categoria? Categoria { get; set; }

    // Armazenamento de imagem (URL do Supabase Storage)
    public string? ImagemUrl { get; set; }

    public ItemInventario() { }

    public ItemInventario(string nome, decimal valorCompra, Guid categoriaId)
    {
        Nome = nome;
        ValorCompra = valorCompra;
        CategoriaId = categoriaId;
    }
}