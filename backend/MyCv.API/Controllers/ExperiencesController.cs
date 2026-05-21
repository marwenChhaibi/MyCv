using MediatR;
using MyCv.Application.Features.Experiences.Commands;
using MyCv.Application.Features.Experiences.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyCv.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExperiencesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetExperiencesQuery(), ct));

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UpsertExperienceCommand cmd, CancellationToken ct)
    {
        var id = await mediator.Send(cmd, ct);
        return Ok(new { id });
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await mediator.Send(new DeleteExperienceCommand(id), ct);
        return ok ? NoContent() : NotFound();
    }
}
