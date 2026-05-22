using MyCv.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.CvDownloads.Queries;

public record CvDownloadDayDto(string Date, int Count);
public record CvDownloadCountryDto(string Country, int Count);
public record CvDownloadDeviceDto(string Device, int Count);
public record CvDownloadUtmSourceDto(string Source, int Count);
public record CvDownloadBrowserDto(string Browser, int Count);

public record CvDownloadStatsDto(
    int Total,
    List<CvDownloadDayDto> Last30Days,
    List<CvDownloadCountryDto> TopCountries,
    List<CvDownloadDeviceDto> DeviceBreakdown,
    List<CvDownloadUtmSourceDto> TopUtmSources,
    List<CvDownloadBrowserDto> BrowserBreakdown);

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

        var topCountries = (await db.CvDownloads
            .Where(d => d.Country != null)
            .GroupBy(d => d.Country!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync(ct))
            .Select(x => new CvDownloadCountryDto(x.Key, x.Count))
            .ToList();

        var deviceBreakdown = (await db.CvDownloads
            .Where(d => d.DeviceType != null)
            .GroupBy(d => d.DeviceType!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .ToListAsync(ct))
            .Select(x => new CvDownloadDeviceDto(x.Key, x.Count))
            .OrderByDescending(x => x.Count)
            .ToList();

        var topUtmSources = (await db.CvDownloads
            .Where(d => d.UtmSource != null)
            .GroupBy(d => d.UtmSource!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync(ct))
            .Select(x => new CvDownloadUtmSourceDto(x.Key, x.Count))
            .ToList();

        var browserBreakdown = (await db.CvDownloads
            .Where(d => d.Browser != null)
            .GroupBy(d => d.Browser!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync(ct))
            .Select(x => new CvDownloadBrowserDto(x.Key, x.Count))
            .ToList();

        return new CvDownloadStatsDto(
            Total:           total,
            Last30Days:      last30,
            TopCountries:    topCountries,
            DeviceBreakdown: deviceBreakdown,
            TopUtmSources:   topUtmSources,
            BrowserBreakdown: browserBreakdown);
    }
}
