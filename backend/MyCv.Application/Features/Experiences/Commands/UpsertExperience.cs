using MediatR;
using MyCv.Application.Common;
using MyCv.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Experiences.Commands;

public record UpsertExperienceCommand(
    Guid? Id,
    string Company,
    string Role,
    string RoleFr,
    string? CompanyUrl,
    string? Location,
    string StartDate,
    string? EndDate,
    bool IsCurrentPosition,
    string Description,
    string DescriptionFr,
    string[] TechStack,
    string[] Highlights,
    string[] HighlightsFr,
    int SortOrder
) : IRequest<Guid>;

public class UpsertExperienceHandler(IAppDbContext db) : IRequestHandler<UpsertExperienceCommand, Guid>
{
    public async Task<Guid> Handle(UpsertExperienceCommand cmd, CancellationToken ct)
    {
        Experience? exp = null;
        if (cmd.Id.HasValue)
            exp = await db.Experiences.FirstOrDefaultAsync(e => e.Id == cmd.Id.Value, ct);

        exp ??= new Experience { Id = Guid.NewGuid() };

        exp.Company = cmd.Company;
        exp.Role = cmd.Role;
        exp.RoleFr = cmd.RoleFr;
        exp.CompanyUrl = cmd.CompanyUrl;
        exp.Location = cmd.Location;
        exp.StartDate = DateOnly.Parse(cmd.StartDate);
        exp.EndDate = cmd.EndDate is not null ? DateOnly.Parse(cmd.EndDate) : null;
        exp.IsCurrentPosition = cmd.IsCurrentPosition;
        exp.Description = cmd.Description;
        exp.DescriptionFr = cmd.DescriptionFr;
        exp.TechStack = cmd.TechStack;
        exp.Highlights = cmd.Highlights;
        exp.HighlightsFr = cmd.HighlightsFr;
        exp.SortOrder = cmd.SortOrder;

        if (!db.Experiences.Local.Contains(exp))
            db.Experiences.Add(exp);

        await db.SaveChangesAsync(ct);
        return exp.Id;
    }
}

public record DeleteExperienceCommand(Guid Id) : IRequest<bool>;

public class DeleteExperienceHandler(IAppDbContext db) : IRequestHandler<DeleteExperienceCommand, bool>
{
    public async Task<bool> Handle(DeleteExperienceCommand cmd, CancellationToken ct)
    {
        var exp = await db.Experiences.FirstOrDefaultAsync(e => e.Id == cmd.Id, ct);
        if (exp is null) return false;
        db.Experiences.Remove(exp);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
