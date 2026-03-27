using AutoMapper;
using Inventario.Application.DTOs;
using Inventario.Domain.Entities;

namespace Inventario.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entidade -> DTO
        CreateMap<ItemInventario, ItemInventarioDTO>()
            .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nome : "Sem Categoria"))
            .ForMember(dest => dest.LocalNome, opt => opt.MapFrom(src => src.Local != null ? src.Local.Nome : "Sem Local"));

        // DTO -> Entidade (Usado na Criação)
        CreateMap<CreateItemInventarioDTO, ItemInventario>();
    }
}