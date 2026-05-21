using MediatR;
using MyCv.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Experiences.Queries;

public record GetExperiencesQuery : IRequest<List<ExperienceDto>>;

public class GetExperiencesHandler(IAppDbContext db) : IRequestHandler<GetExperiencesQuery, List<ExperienceDto>>
{
    public async Task<List<ExperienceDto>> Handle(GetExperiencesQuery _, CancellationToken ct)
    {
        var list = await db.Experiences.OrderBy(e => e.SortOrder).ToListAsync(ct);
        return list.Select(e => new ExperienceDto(
            e.Id, e.Company, e.Role, e.RoleFr, e.CompanyUrl, e.Location,
            e.StartDate.ToString("yyyy-MM"),
            e.EndDate?.ToString("yyyy-MM"),
            e.IsCurrentPosition,
            e.Description, e.DescriptionFr,
            e.TechStack, e.Highlights, e.HighlightsFr,
            e.SortOrder
        )).ToList();
    }
}
