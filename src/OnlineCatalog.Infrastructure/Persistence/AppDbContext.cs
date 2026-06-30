using Microsoft.EntityFrameworkCore;
using OnlineCatalog.Domain.Entities;
using OnlineCatalog.Infrastructure.Persistence.Configurations;

namespace OnlineCatalog.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ApiKeyConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogItemConfiguration());
        modelBuilder.ApplyConfiguration(new WishlistItemConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
