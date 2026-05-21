using MediatR;
using MyCv.Application.Common;
using MyCv.Domain.Entities;

namespace MyCv.Application.Features.Projects.Commands;

public record CreateProjectCommand(
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
) : IRequest<Guid>;

public class CreateProjectHandler(IAppDbContext db) : IRequestHandler<CreateProjectCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectCommand cmd, CancellationToken ct)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = cmd.Title,
            TitleFr = cmd.TitleFr,
            Description = cmd.Description,
            DescriptionFr = cmd.DescriptionFr,
            Type = Enum.Parse<ProjectType>(cmd.Type, ignoreCase: true),
            TechStack = cmd.TechStack,
            LiveUrl = cmd.LiveUrl,
            GithubUrl = cmd.GithubUrl,
            AzureDevOpsUrl = cmd.AzureDevOpsUrl,
            SortOrder = cmd.SortOrder,
            IsVisible = cmd.IsVisible
        };

        db.Projects.Add(project);
        await db.SaveChangesAsync(ct);
        return project.Id;
    }
}
