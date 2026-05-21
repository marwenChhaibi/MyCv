using MediatR;
using MyCv.Application.Features.Skills.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MyCv.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetSkillsQuery(), ct));
}
