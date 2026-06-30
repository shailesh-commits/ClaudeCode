using OnlineCatalog.Domain.Entities;

namespace OnlineCatalog.Domain.Interfaces.Repositories;

public interface ICatalogItemRepository
{
    Task<(List<CatalogItem> Items, int TotalCount)> GetPagedAsync(Guid? categoryId, string? search, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<CatalogItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CatalogItem> AddAsync(CatalogItem item, CancellationToken cancellationToken = default);
    Task UpdateAsync(CatalogItem item, CancellationToken cancellationToken = default);
    Task DeleteAsync(CatalogItem item, CancellationToken cancellationToken = default);
}
