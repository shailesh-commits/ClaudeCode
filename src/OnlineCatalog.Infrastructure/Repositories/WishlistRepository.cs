using Microsoft.EntityFrameworkCore;
using OnlineCatalog.Domain.Entities;
using OnlineCatalog.Domain.Interfaces.Repositories;
using OnlineCatalog.Infrastructure.Persistence;

namespace OnlineCatalog.Infrastructure.Repositories;

public class WishlistRepository(AppDbContext db) : IWishlistRepository
{
    public Task<List<WishlistItem>> GetByUserIdAsync(Guid userId, Guid? categoryId, CancellationToken cancellationToken = default)
    {
        var query = db.WishlistItems
            .Include(w => w.CatalogItem)
            .ThenInclude(i => i.Category)
            .Where(w => w.UserId == userId);

        if (categoryId.HasValue)
            query = query.Where(w => w.CatalogItem.CategoryId == categoryId.Value);

        return query.OrderBy(w => w.AddedAt).ToListAsync(cancellationToken);
    }

    public Task<WishlistItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => db.WishlistItems
            .Include(w => w.CatalogItem)
            .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    public Task<WishlistItem?> GetByUserAndCatalogItemAsync(Guid userId, Guid catalogItemId, CancellationToken cancellationToken = default)
        => db.WishlistItems.FirstOrDefaultAsync(w => w.UserId == userId && w.CatalogItemId == catalogItemId, cancellationToken);

    public async Task<WishlistItem> AddAsync(WishlistItem item, CancellationToken cancellationToken = default)
    {
        db.WishlistItems.Add(item);
        await db.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task DeleteAsync(WishlistItem item, CancellationToken cancellationToken = default)
    {
        db.WishlistItems.Remove(item);
        await db.SaveChangesAsync(cancellationToken);
    }
}
