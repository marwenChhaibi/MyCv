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
    public record RecordVisitRequest(
        string? FingerprintId,
        string? Referrer,
        string? DeviceType,
        string? Language);

    [HttpPost]
    public async Task<IActionResult> Record([FromBody] RecordVisitRequest? body)
    {
        var ua = Request.Headers.UserAgent.ToString();
        var ip = GetClientIp();
        await mediator.Send(new RecordVisitCommand(
            ua,
            body?.FingerprintId,
            ip,
            body?.Referrer,
            body?.DeviceType,
            body?.Language));
        return Ok();
    }

    [HttpGet("stats")]
    [Authorize]
    public async Task<IActionResult> Stats()
    {
        var result = await mediator.Send(new GetVisitStatsQuery());
        return Ok(result);
    }

    private string? GetClientIp()
    {
        var forwarded = Request.Headers["X-Forwarded-For"].ToString();
        if (!string.IsNullOrWhiteSpace(forwarded))
            return forwarded.Split(',')[0].Trim();
        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}
