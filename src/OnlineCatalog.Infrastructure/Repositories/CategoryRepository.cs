using Microsoft.EntityFrameworkCore;
using OnlineCatalog.Domain.Entities;
using OnlineCatalog.Domain.Interfaces.Repositories;
using OnlineCatalog.Infrastructure.Persistence;

namespace OnlineCatalog.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext db) : ICategoryRepository
{
    public Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => db.Categories.OrderBy(c => c.Name).ToListAsync(cancellationToken);

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => db.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        db.Categories.Add(category);
        await db.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        db.Categories.Update(category);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        db.Categories.Remove(category);
        await db.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        => db.Categories.AnyAsync(c => c.Name == name, cancellationToken);

    public Task<bool> HasCatalogItemsAsync(Guid id, CancellationToken cancellationToken = default)
        => db.CatalogItems.AnyAsync(i => i.CategoryId == id, cancellationToken);
}
