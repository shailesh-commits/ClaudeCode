using AutoMapper;
using MediatR;
using OnlineCatalog.Application.Common;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Wishlist.Queries;

public record GetWishlistQuery(Guid? CategoryId) : IRequest<WishlistDto>;

public class GetWishlistQueryHandler(
    IWishlistRepository wishlistRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetWishlistQuery, WishlistDto>
{
    public async Task<WishlistDto> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        var items = await wishlistRepository.GetByUserIdAsync(userId, request.CategoryId, cancellationToken);
        var itemDtos = mapper.Map<List<WishlistItemDto>>(items);
        return new WishlistDto(userId, itemDtos.Count, itemDtos);
    }
}
