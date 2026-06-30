namespace OnlineCatalog.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public ICollection<CatalogItem> CatalogItems { get; private set; } = new List<CatalogItem>();

    private Category() { }

    public static Category Create(string name, string? description = null)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Update(string? name, string? description)
    {
        if (name is not null) Name = name;
        if (description is not null) Description = description;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
