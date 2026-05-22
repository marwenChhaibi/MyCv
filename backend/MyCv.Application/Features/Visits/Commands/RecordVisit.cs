using MyCv.Application.Common;
using MyCv.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Visits.Commands;

public record RecordVisitCommand(
    string? UserAgent,
    string? FingerprintId,
    string? IpAddress,
    string? Referrer,
    string? UtmSource,
    string? UtmMedium,
    string? DeviceType,
    string? Language) : IRequest;

public class RecordVisitHandler(IAppDbContext db, IGeoLocationService geo) : IRequestHandler<RecordVisitCommand>
{
    public async Task Handle(RecordVisitCommand req, CancellationToken ct)
    {
        if (req.FingerprintId is not null)
        {
            var cutoff = DateTime.UtcNow.AddHours(-24);
            var exists = await db.PageVisits
                .AnyAsync(v => v.FingerprintId == req.FingerprintId && v.VisitedAt >= cutoff, ct);
            if (exists) return;
        }

        var country = req.IpAddress is not null
            ? await geo.GetCountryAsync(req.IpAddress, ct)
            : null;

        db.PageVisits.Add(new PageVisit
        {
            VisitedAt     = DateTime.UtcNow,
            UserAgent     = req.UserAgent?[..Math.Min(req.UserAgent.Length, 300)],
            FingerprintId = req.FingerprintId,
            IpAddress     = req.IpAddress,
            Country       = country,
            Referrer      = req.Referrer?[..Math.Min(req.Referrer.Length, 500)],
            UtmSource     = req.UtmSource?[..Math.Min(req.UtmSource.Length, 100)],
            UtmMedium     = req.UtmMedium?[..Math.Min(req.UtmMedium.Length, 100)],
            DeviceType    = req.DeviceType,
            Browser       = UserAgentParser.ParseBrowser(req.UserAgent),
            Language      = req.Language?[..Math.Min(req.Language.Length, 10)],
        });
        await db.SaveChangesAsync(ct);
    }
}
