namespace OnlineCatalog.Domain.Entities;

public class CatalogItem
{
    public Guid Id { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public Category Category { get; private set; } = default!;
    public ICollection<WishlistItem> WishlistItems { get; private set; } = new List<WishlistItem>();

    private CatalogItem() { }

    public static CatalogItem Create(Guid categoryId, string name, string? description, decimal price, string? imageUrl, bool isActive = true)
    {
        return new CatalogItem
        {
            Id = Guid.NewGuid(),
            CategoryId = categoryId,
            Name = name,
            Description = description,
            Price = price,
            ImageUrl = imageUrl,
            IsActive = isActive,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Update(Guid? categoryId, string? name, string? description, decimal? price, string? imageUrl, bool? isActive)
    {
        if (categoryId.HasValue) CategoryId = categoryId.Value;
        if (name is not null) Name = name;
        if (description is not null) Description = description;
        if (price.HasValue) Price = price.Value;
        if (imageUrl is not null) ImageUrl = imageUrl;
        if (isActive.HasValue) IsActive = isActive.Value;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
