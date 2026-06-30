using AutoMapper;
using MediatR;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Catalog.Queries;

public record GetCatalogItemsQuery(
    Guid? CategoryId,
    string? Search,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<CatalogItemDto>>;

public class GetCatalogItemsQueryHandler(
    ICatalogItemRepository catalogItemRepository,
    IMapper mapper) : IRequestHandler<GetCatalogItemsQuery, PagedResult<CatalogItemDto>>
{
    private const int MaxPageSize = 100;

    public async Task<PagedResult<CatalogItemDto>> Handle(GetCatalogItemsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, MaxPageSize);

        var (items, totalCount) = await catalogItemRepository.GetPagedAsync(
            request.CategoryId, request.Search, page, pageSize, cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var dtos = mapper.Map<List<CatalogItemDto>>(items);

        return new PagedResult<CatalogItemDto>(page, pageSize, totalCount, totalPages, dtos);
    }
}
