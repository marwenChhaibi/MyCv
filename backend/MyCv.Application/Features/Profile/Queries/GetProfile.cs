using MediatR;
using MyCv.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Features.Profile.Queries;

public record GetProfileQuery : IRequest<ProfileDto?>;

public class GetProfileHandler(IAppDbContext db) : IRequestHandler<GetProfileQuery, ProfileDto?>
{
    public async Task<ProfileDto?> Handle(GetProfileQuery _, CancellationToken ct)
    {
        var p = await db.Profiles.FirstOrDefaultAsync(ct);
        if (p is null) return null;
        return new ProfileDto(
            p.Id, p.FullName, p.Title, p.TitleFr,
            p.Bio, p.BioFr, p.Email, p.Phone, p.Location,
            p.LinkedInUrl, p.GitHubUrl, p.AzureDevOpsUrl,
            p.AvatarUrl, p.YearsOfExperience, p.OpenToWork
        );
    }
}
