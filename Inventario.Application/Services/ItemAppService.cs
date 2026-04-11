using AutoMapper;
using FluentValidation;
using Inventario.Application.DTOs;
using Inventario.Application.Interfaces;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Services;

public class ItemAppService : IItemAppService
{
    private readonly IValidator<CreateItemInventarioDTO> _validator;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ItemAppService(
        IUnitOfWork uow,
        IValidator<CreateItemInventarioDTO> validator,
        IMapper mapper)
    {
        _uow = uow;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ItemInventarioDTO>> GetAllAsync()
    {
        var itens = await _uow.Itens.GetAllAsync();
        return _mapper.Map<IEnumerable<ItemInventarioDTO>>(itens);
    }

    public async Task<Guid?> AddAsync(CreateItemInventarioDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var item = _mapper.Map<ItemInventario>(dto);

        await _uow.Itens.AddAsync(item);
        var saved = await _uow.CommitAsync() > 0;

        return saved ? item.Id : null;
    }

    public async Task<ItemInventarioDTO?> GetByIdAsync(Guid id)
    {
        var item = await _uow.Itens.GetByIdAsync(id);
        if (item == null) return null;

        return new ItemInventarioDTO(
            item.Id,
            item.Nome,
            item.Descricao,
            item.Marca,
            item.Modelo,
            item.ValorCompra,
            item.ValorAtual,
            item.DataAquisicao,
            item.Categoria?.Nome ?? string.Empty,
            item.Local?.Nome ?? string.Empty,
            item.ImagemUrl,
            item.NotaFiscalUrl);
    }

    public async Task<bool> UpdateAsync(Guid id, CreateItemInventarioDTO dto)
    {
        var itemExistente = await _uow.Itens.GetByIdAsync(id);
        if (itemExistente == null) return false;

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
