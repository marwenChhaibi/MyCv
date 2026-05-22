using MyCv.Application.Common;
using MyCv.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.CvDownloads.Commands;

public record RecordCvDownloadCommand(
    string? UserAgent,
    string? FingerprintId,
    string? IpAddress,
    string? Referrer,
    string? DeviceType,
    string? Language) : IRequest;

public class RecordCvDownloadHandler(IAppDbContext db, IGeoLocationService geo) : IRequestHandler<RecordCvDownloadCommand>
{
    public async Task Handle(RecordCvDownloadCommand req, CancellationToken ct)
    {
        if (req.FingerprintId is not null)
        {
            var cutoff = DateTime.UtcNow.AddHours(-1);
            var exists = await db.CvDownloads
                .AnyAsync(d => d.FingerprintId == req.FingerprintId && d.DownloadedAt >= cutoff, ct);
            if (exists) return;
        }

        var country = req.IpAddress is not null
            ? await geo.GetCountryAsync(req.IpAddress, ct)
            : null;

        db.CvDownloads.Add(new CvDownload
        {
            DownloadedAt  = DateTime.UtcNow,
            UserAgent     = req.UserAgent?[..Math.Min(req.UserAgent.Length, 300)],
            FingerprintId = req.FingerprintId,
            IpAddress     = req.IpAddress,
            Country       = country,
            Referrer      = req.Referrer?[..Math.Min(req.Referrer.Length, 500)],
            DeviceType    = req.DeviceType,
            Browser       = UserAgentParser.ParseBrowser(req.UserAgent),
            Language      = req.Language?[..Math.Min(req.Language.Length, 10)],
        });
        await db.SaveChangesAsync(ct);
    }
}
