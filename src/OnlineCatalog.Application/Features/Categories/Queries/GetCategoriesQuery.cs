using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Categories.Queries;

public record GetCategoriesQuery : IRequest<List<CategoryDto>>;

public class GetCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    IMemoryCache cache,
    IMapper mapper) : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private const string CacheKey = "categories_all";

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(CacheKey, out List<CategoryDto>? cached) && cached is not null)
            return cached;

        var categories = await categoryRepository.GetAllAsync(cancellationToken);
        var result = mapper.Map<List<CategoryDto>>(categories);

        cache.Set(CacheKey, result, TimeSpan.FromSeconds(60));
        return result;
    }
}
