using MyCv.Application.Common;
using MyCv.Domain.Entities;
using MediatR;

namespace MyCv.Application.Features.CvDownloads.Commands;

public record RecordCvDownloadCommand(string? UserAgent) : IRequest;

public class RecordCvDownloadHandler(IAppDbContext db) : IRequestHandler<RecordCvDownloadCommand>
{
    public async Task Handle(RecordCvDownloadCommand req, CancellationToken ct)
    {
        db.CvDownloads.Add(new CvDownload
        {
            DownloadedAt = DateTime.UtcNow,
            UserAgent = req.UserAgent?[..Math.Min(req.UserAgent.Length, 300)],
        });
        await db.SaveChangesAsync(ct);
    }
}
