using MediatR;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Category '{request.Id}' not found.");

        if (await categoryRepository.HasCatalogItemsAsync(request.Id, cancellationToken))
            throw new ConflictException("Cannot delete a category that has catalog items.");

        await categoryRepository.DeleteAsync(category, cancellationToken);
    }
}
