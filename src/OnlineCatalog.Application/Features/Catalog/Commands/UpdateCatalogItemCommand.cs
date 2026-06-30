using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Catalog.Commands;

public record UpdateCatalogItemCommand(
    Guid Id,
    Guid? CategoryId,
    string? Name,
    string? Description,
    decimal? Price,
    string? ImageUrl,
    bool? IsActive) : IRequest<CatalogItemDto>;

public class UpdateCatalogItemCommandValidator : AbstractValidator<UpdateCatalogItemCommand>
{
    public UpdateCatalogItemCommandValidator()
    {
        RuleFor(x => x.Name).MaximumLength(200).When(x => x.Name is not null);
        RuleFor(x => x.Description).MaximumLength(1000).When(x => x.Description is not null);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
        RuleFor(x => x.ImageUrl).Must(u => Uri.TryCreate(u, UriKind.Absolute, out _)).When(x => x.ImageUrl is not null)
            .WithMessage("ImageUrl must be a valid URL.");
    }
}

public class UpdateCatalogItemCommandHandler(
    ICatalogItemRepository catalogItemRepository,
    ICategoryRepository categoryRepository,
    IMapper mapper) : IRequestHandler<UpdateCatalogItemCommand, CatalogItemDto>
{
    public async Task<CatalogItemDto> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
    {
        var item = await catalogItemRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Catalog item '{request.Id}' not found.");

        if (request.CategoryId.HasValue)
        {
            var category = await categoryRepository.GetByIdAsync(request.CategoryId.Value, cancellationToken);
            if (category is null)
                throw new UnprocessableEntityException("categoryId", $"Category '{request.CategoryId}' does not exist.");
        }

        item.Update(request.CategoryId, request.Name, request.Description, request.Price, request.ImageUrl, request.IsActive);
        await catalogItemRepository.UpdateAsync(item, cancellationToken);

        item = (await catalogItemRepository.GetByIdAsync(item.Id, cancellationToken))!;
        return mapper.Map<CatalogItemDto>(item);
    }
}
