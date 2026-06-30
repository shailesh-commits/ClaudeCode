using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCatalog.Application.Features.Wishlist.Commands;
using OnlineCatalog.Application.Features.Wishlist.Queries;
using OnlineCatalog.Infrastructure.Authentication;

namespace OnlineCatalog.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/wishlist")]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
public class WishlistController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWishlist([FromQuery] Guid? categoryId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetWishlistQuery(categoryId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddItem([FromBody] AddWishlistItemCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpDelete("{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItem(Guid itemId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveWishlistItemCommand(itemId), cancellationToken);
        return NoContent();
    }
}
