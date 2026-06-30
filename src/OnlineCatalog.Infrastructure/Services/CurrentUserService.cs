using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OnlineCatalog.Application.Common;

namespace OnlineCatalog.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId
    {
        get
        {
            var claim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim is null || !Guid.TryParse(claim.Value, out var id))
                throw new UnauthorizedAccessException("User is not authenticated.");
            return id;
        }
    }

    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
