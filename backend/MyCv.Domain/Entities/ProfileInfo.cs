namespace MyCv.Domain.Entities;

public class ProfileInfo
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string TitleFr { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string BioFr { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string LinkedInUrl { get; set; } = string.Empty;
    public string? GitHubUrl { get; set; }
    public string? AzureDevOpsUrl { get; set; }
    public string? AvatarUrl { get; set; }
    public int YearsOfExperience { get; set; }
    public bool OpenToWork { get; set; } = true;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
