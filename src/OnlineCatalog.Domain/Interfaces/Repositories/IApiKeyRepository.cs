using OnlineCatalog.Domain.Entities;

namespace OnlineCatalog.Domain.Interfaces.Repositories;

public interface IApiKeyRepository
{
    Task<ApiKey?> GetByHashAsync(string keyHash, CancellationToken cancellationToken = default);
    Task<ApiKey> AddAsync(ApiKey apiKey, CancellationToken cancellationToken = default);
}
