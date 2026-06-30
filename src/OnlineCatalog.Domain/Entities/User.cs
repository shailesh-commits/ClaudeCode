namespace OnlineCatalog.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public ICollection<ApiKey> ApiKeys { get; private set; } = new List<ApiKey>();
    public ICollection<WishlistItem> WishlistItems { get; private set; } = new List<WishlistItem>();

    private User() { }

    public static User Create(string name, string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Update(string? name, string? email)
    {
        if (name is not null) Name = name;
        if (email is not null) Email = email;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
