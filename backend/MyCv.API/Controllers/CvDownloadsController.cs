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
    [HttpPost("downloads")]
    public async Task<IActionResult> Record()
    {
        var ua = Request.Headers.UserAgent.ToString();
        await mediator.Send(new RecordCvDownloadCommand(ua));
        return Ok();
    }

    [HttpGet("downloads/stats")]
    [Authorize]
    public async Task<IActionResult> Stats()
    {
        var result = await mediator.Send(new GetCvDownloadStatsQuery());
        return Ok(result);
    }
}
