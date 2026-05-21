using MediatR;
using MyCv.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Skills.Queries;

public record GetSkillsQuery : IRequest<List<SkillDto>>;

public class GetSkillsHandler(IAppDbContext db) : IRequestHandler<GetSkillsQuery, List<SkillDto>>
{
    public async Task<List<SkillDto>> Handle(GetSkillsQuery _, CancellationToken ct)
    {
        var list = await db.Skills.OrderBy(s => s.SortOrder).ToListAsync(ct);
        return list.Select(s => new SkillDto(s.Id, s.Category, s.CategoryFr, s.Name, s.Level.ToString(), s.SortOrder)).ToList();
    }
}
