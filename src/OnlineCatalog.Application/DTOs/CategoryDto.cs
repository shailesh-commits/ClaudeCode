namespace OnlineCatalog.Application.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);

public record CategorySummaryDto(Guid Id, string Name);
