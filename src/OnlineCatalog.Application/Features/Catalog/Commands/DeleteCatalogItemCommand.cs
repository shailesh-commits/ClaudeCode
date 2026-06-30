using MediatR;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Catalog.Commands;

public record DeleteCatalogItemCommand(Guid Id) : IRequest;

public class DeleteCatalogItemCommandHandler(ICatalogItemRepository catalogItemRepository) : IRequestHandler<DeleteCatalogItemCommand>
{
    public async Task Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
    {
        var item = await catalogItemRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Catalog item '{request.Id}' not found.");

        await catalogItemRepository.DeleteAsync(item, cancellationToken);
    }
}
