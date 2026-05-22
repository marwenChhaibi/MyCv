using MyCv.Application.Features.CvDownloads.Commands;
using MyCv.Application.Features.CvDownloads.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyCv.API.Controllers;

[ApiController]
[Route("api/cv")]
public class CvDownloadsController(IMediator mediator) : ControllerBase
{
    public record RecordDownloadRequest(
        string? FingerprintId,
        string? Referrer,
        string? UtmSource,
        string? UtmMedium,
        string? DeviceType,
        string? Language);

    [HttpPost("downloads")]
    public async Task<IActionResult> Record([FromBody] RecordDownloadRequest? body)
    {
        var ua = Request.Headers.UserAgent.ToString();
        var ip = GetClientIp();
        await mediator.Send(new RecordCvDownloadCommand(
            ua,
            body?.FingerprintId,
            ip,
            body?.Referrer,
            body?.UtmSource,
            body?.UtmMedium,
            body?.DeviceType,
            body?.Language));
        return Ok();
    }

    [HttpGet("downloads/stats")]
    [Authorize]
    public async Task<IActionResult> Stats()
    {
        var result = await mediator.Send(new GetCvDownloadStatsQuery());
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
