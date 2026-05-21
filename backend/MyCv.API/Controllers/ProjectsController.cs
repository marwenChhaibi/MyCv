using MediatR;
using MyCv.Application.Features.Projects.Commands;
using MyCv.Application.Features.Projects.Queries;
using MyCv.Domain.Entities;
using MyCv.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyCv.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController(IMediator mediator, UrfFilesClient filesClient) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? type, CancellationToken ct)
    {
        ProjectType? parsed = Enum.TryParse<ProjectType>(type, true, out var t) ? t : null;
        var result = await mediator.Send(new GetProjectsQuery(parsed), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProjectByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand cmd, CancellationToken ct)
    {
        var id = await mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectCommand cmd, CancellationToken ct)
    {
        if (id != cmd.Id) return BadRequest();
        var ok = await mediator.Send(cmd, ct);
        return ok ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await mediator.Send(new DeleteProjectCommand(id), ct);
        return ok ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpPost("{id:guid}/screenshots")]
    public async Task<IActionResult> UploadScreenshot(Guid id, IFormFile file, [FromForm] string? caption, [FromForm] string? captionFr, [FromForm] int sortOrder, CancellationToken ct)
    {
        await using var stream = file.OpenReadStream();
        var url = await filesClient.UploadAsync(stream, file.FileName, file.ContentType, ct);
        var screenshotId = await mediator.Send(new AddScreenshotCommand(id, url, caption, captionFr, sortOrder), ct);
        return screenshotId is null ? NotFound() : Ok(new { id = screenshotId, url });
    }

    [Authorize]
    [HttpDelete("screenshots/{screenshotId:guid}")]
    public async Task<IActionResult> DeleteScreenshot(Guid screenshotId, CancellationToken ct)
    {
        var ok = await mediator.Send(new DeleteScreenshotCommand(screenshotId), ct);
        return ok ? NoContent() : NotFound();
    }
}
