using MyCv.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.CvDownloads.Queries;

public record CvDownloadDayDto(string Date, int Count);
public record CvDownloadStatsDto(int Total, List<CvDownloadDayDto> Last30Days);

public record GetCvDownloadStatsQuery : IRequest<CvDownloadStatsDto>;

public class GetCvDownloadStatsHandler(IAppDbContext db) : IRequestHandler<GetCvDownloadStatsQuery, CvDownloadStatsDto>
{
    public async Task<CvDownloadStatsDto> Handle(GetCvDownloadStatsQuery _, CancellationToken ct)
    {
        var todayStart = DateTime.UtcNow.Date;
        var monthStart = todayStart.AddDays(-29);

        var recent = await db.CvDownloads
            .Where(d => d.DownloadedAt >= monthStart)
            .ToListAsync(ct);

        var total = await db.CvDownloads.CountAsync(ct);

        var byDay = recent
            .GroupBy(d => d.DownloadedAt.Date)
            .ToDictionary(g => g.Key, g => g.Count());

        var last30 = Enumerable.Range(0, 30)
            .Select(i => todayStart.AddDays(-29 + i))
            .Select(d => new CvDownloadDayDto(d.ToString("yyyy-MM-dd"), byDay.GetValueOrDefault(d, 0)))
            .ToList();

        return new CvDownloadStatsDto(Total: total, Last30Days: last30);
    }
}
