using Homees.Application.Lights.Commands.ConnectLight;
using Homees.Application.Lights.Commands.CreateLight;
using Homees.Application.Lights.Commands.TurnOnLight;
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
    
    [HttpPost("{id:guid}/on")]
    public async Task<IActionResult> TurnOnPost(Guid id)
    {
        await sender.Send(new TurnOnLightCommand(id));

        return Ok();
    }
    
    [HttpPost("{id:guid}/connect")]
    public async Task<IActionResult> ConnectPost(Guid id)
    {
        await sender.Send(new ConnectLightCommand(id));

        return Ok();
    }
}