using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCatalog.Domain.Entities;

namespace OnlineCatalog.Infrastructure.Persistence.Configurations;

public class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(k => k.KeyHash).IsRequired().HasMaxLength(512);
        builder.HasIndex(k => k.KeyHash).IsUnique();
        builder.Property(k => k.Label).HasMaxLength(100);
        builder.Property(k => k.CreatedAt).IsRequired();

        builder.HasOne(k => k.User)
            .WithMany(u => u.ApiKeys)
            .HasForeignKey(k => k.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
