using MediatR;
using MyCv.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Projects.Queries;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto?>;

public class GetProjectByIdHandler(IAppDbContext db) : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken ct)
    {
        var project = await db.Projects
            .Include(p => p.Screenshots)
            .Include(p => p.Features)
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        return project is null ? null : GetProjectsHandler.MapToDto(project);
    }
}
