using MediatR;
using MyCv.Application.Common;
using MyCv.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Projects.Queries;

public record GetProjectsQuery(ProjectType? Type = null) : IRequest<List<ProjectDto>>;

public class GetProjectsHandler(IAppDbContext db) : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
{
    public async Task<List<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken ct)
    {
        var query = db.Projects
            .Include(p => p.Screenshots)
            .Include(p => p.Features)
            .Where(p => p.IsVisible)
            .AsQueryable();

        if (request.Type.HasValue)
            query = query.Where(p => p.Type == request.Type.Value);

        var projects = await query.OrderBy(p => p.SortOrder).ToListAsync(ct);
        return projects.Select(MapToDto).ToList();
    }

    internal static ProjectDto MapToDto(Project p) => new(
        p.Id, p.Title, p.TitleFr,
        p.Description, p.DescriptionFr,
        p.Type.ToString(), p.TechStack,
        p.LiveUrl, p.GithubUrl, p.AzureDevOpsUrl,
        p.SortOrder, p.IsVisible,
        p.Screenshots.OrderBy(s => s.SortOrder).Select(s => new ScreenshotDto(s.Id, s.Url, s.Caption, s.CaptionFr, s.SortOrder)).ToList(),
        p.Features.OrderBy(f => f.SortOrder).Select(f => new FeatureDto(f.Id, f.Label, f.LabelFr, f.SortOrder)).ToList()
    );
}
