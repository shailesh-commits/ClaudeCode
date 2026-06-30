namespace OnlineCatalog.Application.DTOs;

public record WishlistDto(
    Guid UserId,
    int TotalCount,
    List<WishlistItemDto> Items
);

public record WishlistItemDto(
    Guid Id,
    DateTimeOffset AddedAt,
    CatalogItemSummaryDto CatalogItem
);

public record CatalogItemSummaryDto(
    Guid Id,
    string Name,
    decimal Price,
    string? ImageUrl,
    CategorySummaryDto Category
);
