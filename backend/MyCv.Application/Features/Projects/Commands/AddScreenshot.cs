using MediatR;
using MyCv.Application.Common;
using MyCv.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Projects.Commands;

public record AddScreenshotCommand(Guid ProjectId, string Url, string? Caption, string? CaptionFr, int SortOrder) : IRequest<Guid?>;

public class AddScreenshotHandler(IAppDbContext db) : IRequestHandler<AddScreenshotCommand, Guid?>
{
    public async Task<Guid?> Handle(AddScreenshotCommand cmd, CancellationToken ct)
    {
        var exists = await db.Projects.AnyAsync(p => p.Id == cmd.ProjectId, ct);
        if (!exists) return null;

        var screenshot = new ProjectScreenshot
        {
            Id = Guid.NewGuid(),
            ProjectId = cmd.ProjectId,
            Url = cmd.Url,
            Caption = cmd.Caption,
            CaptionFr = cmd.CaptionFr,
            SortOrder = cmd.SortOrder
        };

        db.ProjectScreenshots.Add(screenshot);
        await db.SaveChangesAsync(ct);
        return screenshot.Id;
    }
}

public record DeleteScreenshotCommand(Guid Id) : IRequest<bool>;

public class DeleteScreenshotHandler(IAppDbContext db) : IRequestHandler<DeleteScreenshotCommand, bool>
{
    public async Task<bool> Handle(DeleteScreenshotCommand cmd, CancellationToken ct)
    {
        var s = await db.ProjectScreenshots.FirstOrDefaultAsync(s => s.Id == cmd.Id, ct);
        if (s is null) return false;
        db.ProjectScreenshots.Remove(s);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
