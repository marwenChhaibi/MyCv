using MediatR;
using MyCv.Application.Common;
using MyCv.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Profile.Commands;

public record UpdateProfileCommand(
    string FullName,
    string Title,
    string TitleFr,
    string Bio,
    string BioFr,
    string Email,
    string Phone,
    string Location,
    string LinkedInUrl,
    string? GitHubUrl,
    string? AzureDevOpsUrl,
    string? AvatarUrl,
    int YearsOfExperience,
    bool OpenToWork
) : IRequest<Guid>;

public class UpdateProfileHandler(IAppDbContext db) : IRequestHandler<UpdateProfileCommand, Guid>
{
    public async Task<Guid> Handle(UpdateProfileCommand cmd, CancellationToken ct)
    {
        var profile = await db.Profiles.FirstOrDefaultAsync(ct) ?? new ProfileInfo { Id = Guid.NewGuid() };

        profile.FullName = cmd.FullName;
        profile.Title = cmd.Title;
        profile.TitleFr = cmd.TitleFr;
        profile.Bio = cmd.Bio;
        profile.BioFr = cmd.BioFr;
        profile.Email = cmd.Email;
        profile.Phone = cmd.Phone;
        profile.Location = cmd.Location;
        profile.LinkedInUrl = cmd.LinkedInUrl;
        profile.GitHubUrl = cmd.GitHubUrl;
        profile.AzureDevOpsUrl = cmd.AzureDevOpsUrl;
        profile.AvatarUrl = cmd.AvatarUrl;
        profile.YearsOfExperience = cmd.YearsOfExperience;
        profile.OpenToWork = cmd.OpenToWork;
        profile.UpdatedAt = DateTime.UtcNow;

        if (!db.Profiles.Local.Contains(profile))
            db.Profiles.Add(profile);

        await db.SaveChangesAsync(ct);
        return profile.Id;
    }
}
