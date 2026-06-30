using Microsoft.EntityFrameworkCore;
using OnlineCatalog.Domain.Entities;
using OnlineCatalog.Domain.Interfaces.Repositories;
using OnlineCatalog.Infrastructure.Persistence;

namespace OnlineCatalog.Infrastructure.Repositories;

public class CatalogItemRepository(AppDbContext db) : ICatalogItemRepository
{
    public async Task<(List<CatalogItem> Items, int TotalCount)> GetPagedAsync(
        Guid? categoryId, string? search, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = db.CatalogItems.Include(i => i.Category).AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(i => i.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(i => i.Name.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(i => i.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<CatalogItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => db.CatalogItems.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task<CatalogItem> AddAsync(CatalogItem item, CancellationToken cancellationToken = default)
    {
        db.CatalogItems.Add(item);
        await db.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task UpdateAsync(CatalogItem item, CancellationToken cancellationToken = default)
    {
        db.CatalogItems.Update(item);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(CatalogItem item, CancellationToken cancellationToken = default)
    {
        db.CatalogItems.Remove(item);
        await db.SaveChangesAsync(cancellationToken);
    }
}
