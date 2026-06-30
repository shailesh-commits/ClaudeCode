using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCatalog.Domain.Entities;

namespace OnlineCatalog.Infrastructure.Persistence.Configurations;

public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
{
    public void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.AddedAt).IsRequired();
        builder.HasIndex(w => new { w.UserId, w.CatalogItemId }).IsUnique();

        builder.HasOne(w => w.User)
            .WithMany(u => u.WishlistItems)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(w => w.CatalogItem)
            .WithMany(i => i.WishlistItems)
            .HasForeignKey(w => w.CatalogItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
