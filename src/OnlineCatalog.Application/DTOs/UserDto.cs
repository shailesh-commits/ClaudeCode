namespace OnlineCatalog.Application.DTOs;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);

public record CreateUserResponse(
    Guid Id,
    string Name,
    string Email,
    DateTimeOffset CreatedAt,
    string ApiKey
);
