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

    public async Task<bool> UpdateAsync(Guid id, CreateItemInventarioDTO dto)
    {
        var itemExistente = await _uow.Itens.GetByIdAsync(id);
        if (itemExistente == null) return false;

        // Atualizando as propriedades (Clean Code: mapeamento manual)
        itemExistente.Nome = dto.Nome;
        itemExistente.Descricao = dto.Descricao;
        itemExistente.Marca = dto.Marca;
        itemExistente.Modelo = dto.Modelo;
        itemExistente.ValorCompra = dto.ValorCompra;
        itemExistente.CategoriaId = dto.CategoriaId;
        itemExistente.LocalId = dto.LocalId;
        itemExistente.DataAquisicao = dto.DataAquisicao;
        itemExistente.DataAtualizacao = DateTime.UtcNow;

        _uow.Itens.Update(itemExistente);
        return await _uow.CommitAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _uow.Itens.GetByIdAsync(id);
        if (item == null) return false;

        _uow.Itens.Delete(item);
        return await _uow.CommitAsync() > 0;
    }
}