using AutoMapper;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Entities;

namespace OnlineCatalog.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<Category, CategoryDto>();
        CreateMap<Category, CategorySummaryDto>();

        CreateMap<CatalogItem, CatalogItemDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

        CreateMap<CatalogItem, CatalogItemSummaryDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

        CreateMap<WishlistItem, WishlistItemDto>()
            .ForMember(dest => dest.CatalogItem, opt => opt.MapFrom(src => src.CatalogItem));
    }
}
