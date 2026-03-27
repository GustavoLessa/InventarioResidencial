using Inventario.Application.DTOs;
using Inventario.Application.Interfaces;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Services;

public class ItemAppService : IItemAppService
{
    private readonly IUnitOfWork _uow;

    public ItemAppService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<ItemInventarioDTO>> GetAllAsync()
    {
        var itens = await _uow.Itens.GetAllAsync();
        
        // Mapeamento manual (depois podemos usar AutoMapper para simplificar)
        return itens.Select(i => new ItemInventarioDTO(
            i.Id, i.Nome, i.Descricao, i.Marca, i.Modelo, 
            i.ValorCompra, i.ValorAtual, i.DataAquisicao,
            i.Categoria?.Nome ?? "Sem Categoria",
            i.Local?.Nome ?? "Sem Local",
            i.ImagemUrl, i.NotaFiscalUrl
        ));
    }

    public async Task<bool> AddAsync(CreateItemInventarioDTO dto)
    {
        var item = new ItemInventario(dto.Nome, dto.ValorCompra, dto.CategoriaId, dto.LocalId)
        {
            Descricao = dto.Descricao,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            DataAquisicao = dto.DataAquisicao
        };

        await _uow.Itens.AddAsync(item);
        return await _uow.CommitAsync() > 0;
    }

    public async Task<ItemInventarioDTO?> GetByIdAsync(Guid id)
    {
        var i = await _uow.Itens.GetByIdAsync(id);
        if (i == null) return null;

        return new ItemInventarioDTO(
            i.Id, i.Nome, i.Descricao, i.Marca, i.Modelo, 
            i.ValorCompra, i.ValorAtual, i.DataAquisicao,
            i.Categoria?.Nome ?? "", i.Local?.Nome ?? "",
            i.ImagemUrl, i.NotaFiscalUrl
        );
    }
}