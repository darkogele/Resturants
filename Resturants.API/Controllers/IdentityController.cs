using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPatch]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}