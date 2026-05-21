using MyCv.Application.Common;
using MyCv.Domain.Entities;
using MediatR;

namespace MyCv.Application.Features.Visits.Commands;

public record RecordVisitCommand(string? UserAgent) : IRequest;

public class RecordVisitHandler(IAppDbContext db) : IRequestHandler<RecordVisitCommand>
{
    public async Task Handle(RecordVisitCommand req, CancellationToken ct)
    {
        db.PageVisits.Add(new PageVisit
        {
            VisitedAt = DateTime.UtcNow,
            UserAgent = req.UserAgent?[..Math.Min(req.UserAgent.Length, 300)],
        });
        await db.SaveChangesAsync(ct);
    }
}
