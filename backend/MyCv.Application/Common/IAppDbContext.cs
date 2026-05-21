using MyCv.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Application.Common;

public interface IAppDbContext
{
    DbSet<Project> Projects { get; }
    DbSet<ProjectScreenshot> ProjectScreenshots { get; }
    DbSet<ProjectFeature> ProjectFeatures { get; }
    DbSet<Experience> Experiences { get; }
    DbSet<Skill> Skills { get; }
    DbSet<ProfileInfo> Profiles { get; }
    DbSet<PageVisit> PageVisits { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
