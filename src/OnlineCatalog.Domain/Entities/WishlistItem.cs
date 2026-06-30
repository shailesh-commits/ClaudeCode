namespace OnlineCatalog.Domain.Entities;

public class WishlistItem
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CatalogItemId { get; private set; }
    public DateTimeOffset AddedAt { get; private set; }

    public User User { get; private set; } = default!;
    public CatalogItem CatalogItem { get; private set; } = default!;

    private WishlistItem() { }

    public static WishlistItem Create(Guid userId, Guid catalogItemId)
    {
        return new WishlistItem
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CatalogItemId = catalogItemId,
            AddedAt = DateTimeOffset.UtcNow
        };
    }
}
