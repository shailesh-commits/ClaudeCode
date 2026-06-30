using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCatalog.Infrastructure.Persistence;

namespace OnlineCatalog.Api.Controllers;

[ApiController]
[Route("health")]
[AllowAnonymous]
public class HealthController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var canConnect = await db.Database.CanConnectAsync(cancellationToken);
        return Ok(new
        {
            status = canConnect ? "healthy" : "degraded",
            database = canConnect ? "connected" : "unavailable",
            timestamp = DateTimeOffset.UtcNow
        });
    }
}
