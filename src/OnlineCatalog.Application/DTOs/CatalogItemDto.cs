namespace OnlineCatalog.Application.DTOs;

public record CatalogItemDto(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    string? ImageUrl,
    bool IsActive,
    CategorySummaryDto Category,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);

public record PagedResult<T>(
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    List<T> Items
);
