namespace Inventario.Application.DTOs;

public record CreateItemInventarioDTO(
    string Nome,
    string? Descricao,
    string? Marca,
    string? Modelo,
    decimal ValorCompra,
    Guid CategoriaId,
    Guid LocalId,
    DateTime? DataAquisicao
);