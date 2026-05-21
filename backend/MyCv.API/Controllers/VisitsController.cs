using MyCv.Application.Features.Visits.Commands;
using MyCv.Application.Features.Visits.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyCv.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Record()
    {
        var ua = Request.Headers.UserAgent.ToString();
        await mediator.Send(new RecordVisitCommand(ua));
        return Ok();
    }

    [HttpGet("stats")]
    [Authorize]
    public async Task<IActionResult> Stats()
    {
        var result = await mediator.Send(new GetVisitStatsQuery());
        return Ok(result);
    }
}
