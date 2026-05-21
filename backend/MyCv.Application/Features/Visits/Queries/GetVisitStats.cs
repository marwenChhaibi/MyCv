using MyCv.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Visits.Queries;

public record VisitDayDto(string Date, int Count);

public record VisitStatsDto(int Total, int Today, int ThisWeek, List<VisitDayDto> Last30Days);

public record GetVisitStatsQuery : IRequest<VisitStatsDto>;

public class GetVisitStatsHandler(IAppDbContext db) : IRequestHandler<GetVisitStatsQuery, VisitStatsDto>
{
    public async Task<VisitStatsDto> Handle(GetVisitStatsQuery _, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var todayStart = now.Date;
        var weekStart = todayStart.AddDays(-6);
        var monthStart = todayStart.AddDays(-29);

        var all = await db.PageVisits
            .Where(v => v.VisitedAt >= monthStart)
            .ToListAsync(ct);

        var total = await db.PageVisits.CountAsync(ct);

        var byDay = all
            .GroupBy(v => v.VisitedAt.Date)
            .ToDictionary(g => g.Key, g => g.Count());

        var last30 = Enumerable.Range(0, 30)
            .Select(i => todayStart.AddDays(-29 + i))
            .Select(d => new VisitDayDto(
                d.ToString("yyyy-MM-dd"),
                byDay.GetValueOrDefault(d, 0)))
            .ToList();

        return new VisitStatsDto(
            Total: total,
            Today: byDay.GetValueOrDefault(todayStart, 0),
            ThisWeek: all.Count(v => v.VisitedAt.Date >= weekStart),
            Last30Days: last30);
    }
}
