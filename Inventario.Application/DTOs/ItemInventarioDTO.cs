namespace Inventario.Application.DTOs;

public record ItemInventarioDTO(
    Guid Id,
    string Nome,
    string? Descricao,
    string? Marca,
    string? Modelo,
    decimal ValorCompra,
    decimal? ValorAtual,
    DateTime? DataAquisicao,
    string CategoriaNome,
    string LocalNome,
    string? ImagemUrl,
    string? NotaFiscalUrl
);