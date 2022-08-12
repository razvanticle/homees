using Homees.Application.Lights.Commands.CreateLight;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Homees.WebAPI.Controllers;

[ApiController]
[Route("api/lights")]
public class LightsController : ControllerBase
{
    private readonly ISender sender;

    public LightsController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateLightCommand command)
    {
        var id = await sender.Send(command);
        
        return Created($"/api/lights/{id}",id);
    }
}