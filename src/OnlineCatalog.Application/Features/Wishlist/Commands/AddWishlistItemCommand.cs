using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineCatalog.Application.Common;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Entities;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Wishlist.Commands;

public record AddWishlistItemCommand(Guid CatalogItemId) : IRequest<WishlistItemDto>;

public class AddWishlistItemCommandValidator : AbstractValidator<AddWishlistItemCommand>
{
    public AddWishlistItemCommandValidator()
    {
        RuleFor(x => x.CatalogItemId).NotEmpty();
    }
}

public class AddWishlistItemCommandHandler(
    IWishlistRepository wishlistRepository,
    ICatalogItemRepository catalogItemRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<AddWishlistItemCommand, WishlistItemDto>
{
    public async Task<WishlistItemDto> Handle(AddWishlistItemCommand request, CancellationToken cancellationToken)
    {
        var catalogItem = await catalogItemRepository.GetByIdAsync(request.CatalogItemId, cancellationToken);
        if (catalogItem is null)
            throw new NotFoundException($"Catalog item '{request.CatalogItemId}' not found.");

        var userId = currentUserService.UserId;
        var existing = await wishlistRepository.GetByUserAndCatalogItemAsync(userId, request.CatalogItemId, cancellationToken);
        if (existing is not null)
            throw new ConflictException("Item is already in your wishlist.");

        var item = WishlistItem.Create(userId, request.CatalogItemId);
        await wishlistRepository.AddAsync(item, cancellationToken);

        item = (await wishlistRepository.GetByIdAsync(item.Id, cancellationToken))!;
        return mapper.Map<WishlistItemDto>(item);
    }
}
