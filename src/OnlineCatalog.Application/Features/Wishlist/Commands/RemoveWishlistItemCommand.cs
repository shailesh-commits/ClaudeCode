using MediatR;
using OnlineCatalog.Application.Common;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Wishlist.Commands;

public record RemoveWishlistItemCommand(Guid ItemId) : IRequest;

public class RemoveWishlistItemCommandHandler(
    IWishlistRepository wishlistRepository,
    ICurrentUserService currentUserService) : IRequestHandler<RemoveWishlistItemCommand>
{
    public async Task Handle(RemoveWishlistItemCommand request, CancellationToken cancellationToken)
    {
        var item = await wishlistRepository.GetByIdAsync(request.ItemId, cancellationToken)
            ?? throw new NotFoundException($"Wishlist item '{request.ItemId}' not found.");

        if (item.UserId != currentUserService.UserId)
            throw new ForbiddenException();

        await wishlistRepository.DeleteAsync(item, cancellationToken);
    }
}
