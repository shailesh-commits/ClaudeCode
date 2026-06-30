using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Entities;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Catalog.Commands;

public record CreateCatalogItemCommand(
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    string? ImageUrl,
    bool IsActive = true) : IRequest<CatalogItemDto>;

public class CreateCatalogItemCommandValidator : AbstractValidator<CreateCatalogItemCommand>
{
    public CreateCatalogItemCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000).When(x => x.Description is not null);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ImageUrl).Must(u => Uri.TryCreate(u, UriKind.Absolute, out _)).When(x => x.ImageUrl is not null)
            .WithMessage("ImageUrl must be a valid URL.");
    }
}

public class CreateCatalogItemCommandHandler(
    ICatalogItemRepository catalogItemRepository,
    ICategoryRepository categoryRepository,
    IMapper mapper) : IRequestHandler<CreateCatalogItemCommand, CatalogItemDto>
{
    public async Task<CatalogItemDto> Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null)
            throw new UnprocessableEntityException("categoryId", $"Category '{request.CategoryId}' does not exist.");

        var item = CatalogItem.Create(request.CategoryId, request.Name, request.Description, request.Price, request.ImageUrl, request.IsActive);
        await catalogItemRepository.AddAsync(item, cancellationToken);

        item = (await catalogItemRepository.GetByIdAsync(item.Id, cancellationToken))!;
        return mapper.Map<CatalogItemDto>(item);
    }
}
