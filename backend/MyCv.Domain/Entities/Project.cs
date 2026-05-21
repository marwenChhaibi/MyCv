namespace MyCv.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TitleFr { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionFr { get; set; } = string.Empty;
    public ProjectType Type { get; set; }
    public string[] TechStack { get; set; } = [];
    public string? LiveUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? AzureDevOpsUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsVisible { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ProjectScreenshot> Screenshots { get; set; } = [];
    public ICollection<ProjectFeature> Features { get; set; } = [];
}

public enum ProjectType
{
    Personal,
    Professional
}
