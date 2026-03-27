using AutoMapper;
using FluentValidation;
using Inventario.Application.DTOs;
using Inventario.Application.Interfaces;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces;

namespace Inventario.Application.Services;


public class ItemAppService : IItemAppService
{
    // Adicione a injeção do IValidator no construtor
private readonly IValidator<CreateItemInventarioDTO> _validator;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper; // Injetamos o Mapper

    public ItemAppService(IUnitOfWork uow, IValidator<CreateItemInventarioDTO> validator, IMapper mapper)
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

    public async Task<bool> AddAsync(CreateItemInventarioDTO dto)
    {
        // Executa a validação
        var validationResult = await _validator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            // Agora lançamos a ValidationException do FluentValidation
            throw new ValidationException(validationResult.Errors);
        }

        // Converte DTO para Entidade automaticamente
        var item = _mapper.Map<ItemInventario>(dto);

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