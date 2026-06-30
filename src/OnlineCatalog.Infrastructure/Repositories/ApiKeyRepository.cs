using Microsoft.EntityFrameworkCore;
using OnlineCatalog.Domain.Entities;
using OnlineCatalog.Domain.Interfaces.Repositories;
using OnlineCatalog.Infrastructure.Persistence;

namespace OnlineCatalog.Infrastructure.Repositories;

public class ApiKeyRepository(AppDbContext db) : IApiKeyRepository
{
    public Task<ApiKey?> GetByHashAsync(string keyHash, CancellationToken cancellationToken = default)
        => db.ApiKeys.FirstOrDefaultAsync(k => k.KeyHash == keyHash, cancellationToken);

    public async Task<ApiKey> AddAsync(ApiKey apiKey, CancellationToken cancellationToken = default)
    {
        db.ApiKeys.Add(apiKey);
        await db.SaveChangesAsync(cancellationToken);
        return apiKey;
    }
}
