namespace MyCv.Application.Common;

public record ProjectDto(
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
    bool IsVisible,
    List<ScreenshotDto> Screenshots,
    List<FeatureDto> Features
);

public record ScreenshotDto(Guid Id, string Url, string? Caption, string? CaptionFr, int SortOrder);
public record FeatureDto(Guid Id, string Label, string LabelFr, int SortOrder);

public record ExperienceDto(
    Guid Id,
    string Company,
    string Role,
    string RoleFr,
    string? CompanyUrl,
    string? Location,
    string StartDate,
    string? EndDate,
    bool IsCurrentPosition,
    string Description,
    string DescriptionFr,
    string[] TechStack,
    string[] Highlights,
    string[] HighlightsFr,
    int SortOrder
);

public record SkillDto(Guid Id, string Category, string CategoryFr, string Name, string Level, int SortOrder);

public record ProfileDto(
    Guid Id,
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
);
