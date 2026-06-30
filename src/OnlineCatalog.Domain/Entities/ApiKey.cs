namespace OnlineCatalog.Domain.Entities;

public class ApiKey
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string KeyHash { get; private set; } = default!;
    public string? Label { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public User User { get; private set; } = default!;

    private ApiKey() { }

    public static ApiKey Create(Guid userId, string keyHash, string? label = null, DateTimeOffset? expiresAt = null)
    {
        return new ApiKey
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            KeyHash = keyHash,
            Label = label,
            ExpiresAt = expiresAt,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public bool IsValid() =>
        RevokedAt is null &&
        (ExpiresAt is null || ExpiresAt > DateTimeOffset.UtcNow);
}
