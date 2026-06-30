using AutoMapper;
using MediatR;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Catalog.Queries;

public record GetCatalogItemByIdQuery(Guid Id) : IRequest<CatalogItemDto>;

public class GetCatalogItemByIdQueryHandler(
    ICatalogItemRepository catalogItemRepository,
    IMapper mapper) : IRequestHandler<GetCatalogItemByIdQuery, CatalogItemDto>
{
    public async Task<CatalogItemDto> Handle(GetCatalogItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await catalogItemRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Catalog item '{request.Id}' not found.");

        return mapper.Map<CatalogItemDto>(item);
    }
}
