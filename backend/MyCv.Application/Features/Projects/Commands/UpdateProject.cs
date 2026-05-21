using MediatR;
using MyCv.Application.Common;
using MyCv.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Projects.Commands;

public record UpdateProjectCommand(
    Guid Id,
    string Title,
    string TitleFr,
    string Description,
    string DescriptionFr,
    string Type,
    string[] TechStack,
    string? LiveUrl,
    string? GithubUrl,
    string? AzureDevOpsUrl,
    int SortOrder,
    bool IsVisible
) : IRequest<bool>;

public class UpdateProjectHandler(IAppDbContext db) : IRequestHandler<UpdateProjectCommand, bool>
{
    public async Task<bool> Handle(UpdateProjectCommand cmd, CancellationToken ct)
    {
        var project = await db.Projects.FirstOrDefaultAsync(p => p.Id == cmd.Id, ct);
        if (project is null) return false;

        project.Title = cmd.Title;
        project.TitleFr = cmd.TitleFr;
        project.Description = cmd.Description;
        project.DescriptionFr = cmd.DescriptionFr;
        project.Type = Enum.Parse<ProjectType>(cmd.Type, ignoreCase: true);
        project.TechStack = cmd.TechStack;
        project.LiveUrl = cmd.LiveUrl;
        project.GithubUrl = cmd.GithubUrl;
        project.AzureDevOpsUrl = cmd.AzureDevOpsUrl;
        project.SortOrder = cmd.SortOrder;
        project.IsVisible = cmd.IsVisible;
        project.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return true;
    }
}
