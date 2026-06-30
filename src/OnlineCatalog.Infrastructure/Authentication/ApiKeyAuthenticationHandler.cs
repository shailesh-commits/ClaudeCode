using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Infrastructure.Authentication;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IApiKeyRepository apiKeyRepository)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "ApiKey";
    private const string HeaderName = "X-Api-Key";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderName, out var keyValues))
            return AuthenticateResult.Fail("Missing X-Api-Key header.");

        var rawKey = keyValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(rawKey))
            return AuthenticateResult.Fail("Empty X-Api-Key header.");

        var keyHash = ComputeSha256(rawKey);
        var apiKey = await apiKeyRepository.GetByHashAsync(keyHash, Context.RequestAborted);

        if (apiKey is null || !apiKey.IsValid())
            return AuthenticateResult.Fail("Invalid or expired API key.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, apiKey.UserId.ToString()),
            new Claim(ClaimTypes.Name, apiKey.UserId.ToString())
        };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return AuthenticateResult.Success(ticket);
    }

    private static string ComputeSha256(string key)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
