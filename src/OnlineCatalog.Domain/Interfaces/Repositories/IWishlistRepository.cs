using OnlineCatalog.Domain.Entities;

namespace OnlineCatalog.Domain.Interfaces.Repositories;

public interface IWishlistRepository
{
    Task<List<WishlistItem>> GetByUserIdAsync(Guid userId, Guid? categoryId, CancellationToken cancellationToken = default);
    Task<WishlistItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<WishlistItem?> GetByUserAndCatalogItemAsync(Guid userId, Guid catalogItemId, CancellationToken cancellationToken = default);
    Task<WishlistItem> AddAsync(WishlistItem item, CancellationToken cancellationToken = default);
    Task DeleteAsync(WishlistItem item, CancellationToken cancellationToken = default);
}
