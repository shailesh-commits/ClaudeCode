using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCatalog.Domain.Entities;

namespace OnlineCatalog.Infrastructure.Persistence.Configurations;

public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name).IsRequired().HasMaxLength(200);
        builder.Property(i => i.Description).HasMaxLength(1000);
        builder.Property(i => i.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(i => i.ImageUrl).HasMaxLength(2048);
        builder.Property(i => i.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(i => i.CreatedAt).IsRequired();

        builder.HasOne(i => i.Category)
            .WithMany(c => c.CatalogItems)
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
