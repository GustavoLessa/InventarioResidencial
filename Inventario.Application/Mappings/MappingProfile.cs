using AutoMapper;
using Inventario.Application.DTOs;
using Inventario.Domain.Entities;

namespace Inventario.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ItemInventario, ItemInventarioDTO>()
            .ForCtorParam(nameof(ItemInventarioDTO.UserId), opt => opt.MapFrom(src => src.UserId))
            .ForCtorParam(nameof(ItemInventarioDTO.CategoriaNome), opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nome : "Sem Categoria"))
            .ForCtorParam(nameof(ItemInventarioDTO.LocalNome), opt => opt.MapFrom(src => src.Local != null ? src.Local.Nome : "Sem Local"));

        CreateMap<CreateItemInventarioDTO, ItemInventario>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
    }
}
