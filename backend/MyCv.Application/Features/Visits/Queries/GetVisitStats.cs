using MyCv.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Visits.Queries;

public record VisitDayDto(string Date, int Count);
public record CountryCountDto(string Country, int Count);
public record ReferrerCountDto(string Referrer, int Count);
public record UtmSourceCountDto(string Source, int Count);
public record DeviceCountDto(string Device, int Count);
public record BrowserCountDto(string Browser, int Count);
public record LanguageCountDto(string Language, int Count);
public record HourCountDto(int Hour, int Count);
public record DayOfWeekCountDto(string Day, int Count);

public record VisitStatsDto(
    int Total,
    int Today,
    int ThisWeek,
    List<VisitDayDto> Last30Days,
    List<CountryCountDto> TopCountries,
    List<ReferrerCountDto> TopReferrers,
    List<UtmSourceCountDto> TopUtmSources,
    List<DeviceCountDto> DeviceBreakdown,
    List<BrowserCountDto> BrowserBreakdown,
    List<LanguageCountDto> TopLanguages,
    List<HourCountDto> VisitsByHour,
    List<DayOfWeekCountDto> VisitsByDayOfWeek);

public record GetVisitStatsQuery : IRequest<VisitStatsDto>;

public class GetVisitStatsHandler(IAppDbContext db) : IRequestHandler<GetVisitStatsQuery, VisitStatsDto>
{
    private static readonly string[] DayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

    public async Task<VisitStatsDto> Handle(GetVisitStatsQuery _, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var todayStart = now.Date;
        var weekStart  = todayStart.AddDays(-6);
        var monthStart = todayStart.AddDays(-29);

        // Last 30 days in-memory (used for bar chart + hour/weekday aggregations)
        var recent = await db.PageVisits
            .Where(v => v.VisitedAt >= monthStart)
            .ToListAsync(ct);

        var total = await db.PageVisits.CountAsync(ct);

        // Bar chart
        var byDay = recent
            .GroupBy(v => v.VisitedAt.Date)
            .ToDictionary(g => g.Key, g => g.Count());

        var last30 = Enumerable.Range(0, 30)
            .Select(i => todayStart.AddDays(-29 + i))
            .Select(d => new VisitDayDto(d.ToString("yyyy-MM-dd"), byDay.GetValueOrDefault(d, 0)))
            .ToList();

        // Hour-of-day distribution (0-23)
        var hourMap = recent
            .GroupBy(v => v.VisitedAt.Hour)
            .ToDictionary(g => g.Key, g => g.Count());
        var visitsByHour = Enumerable.Range(0, 24)
            .Select(h => new HourCountDto(h, hourMap.GetValueOrDefault(h, 0)))
            .ToList();

        // Day-of-week distribution
        var dowMap = recent
            .GroupBy(v => (int)v.VisitedAt.DayOfWeek)
            .ToDictionary(g => g.Key, g => g.Count());
        var visitsByDow = Enumerable.Range(0, 7)
            .Select(d => new DayOfWeekCountDto(DayNames[d], dowMap.GetValueOrDefault(d, 0)))
            .ToList();

        // --- DB-side group-bys (anonymous type avoids EF record-constructor translation bug) ---

        var topCountries = (await db.PageVisits
            .Where(v => v.Country != null)
            .GroupBy(v => v.Country!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync(ct))
            .Select(x => new CountryCountDto(x.Key, x.Count))
            .ToList();

        var topReferrers = (await db.PageVisits
            .Where(v => v.Referrer != null && v.Referrer != "")
            .GroupBy(v => v.Referrer!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync(ct))
            .Select(x => new ReferrerCountDto(x.Key, x.Count))
            .ToList();

        var topUtmSources = (await db.PageVisits
            .Where(v => v.UtmSource != null)
            .GroupBy(v => v.UtmSource!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync(ct))
            .Select(x => new UtmSourceCountDto(x.Key, x.Count))
            .ToList();

        var deviceBreakdown = (await db.PageVisits
            .Where(v => v.DeviceType != null)
            .GroupBy(v => v.DeviceType!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .ToListAsync(ct))
            .Select(x => new DeviceCountDto(x.Key, x.Count))
            .OrderByDescending(x => x.Count)
            .ToList();

        var browserBreakdown = (await db.PageVisits
            .Where(v => v.Browser != null)
            .GroupBy(v => v.Browser!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync(ct))
            .Select(x => new BrowserCountDto(x.Key, x.Count))
            .ToList();

        var topLanguages = (await db.PageVisits
            .Where(v => v.Language != null)
            .GroupBy(v => v.Language!)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync(ct))
            .Select(x => new LanguageCountDto(x.Key, x.Count))
            .ToList();

        return new VisitStatsDto(
            Total:            total,
            Today:            byDay.GetValueOrDefault(todayStart, 0),
            ThisWeek:         recent.Count(v => v.VisitedAt.Date >= weekStart),
            Last30Days:       last30,
            TopCountries:     topCountries,
            TopReferrers:     topReferrers,
            TopUtmSources:    topUtmSources,
            DeviceBreakdown:  deviceBreakdown,
            BrowserBreakdown: browserBreakdown,
            TopLanguages:     topLanguages,
            VisitsByHour:     visitsByHour,
            VisitsByDayOfWeek: visitsByDow);
    }
}
