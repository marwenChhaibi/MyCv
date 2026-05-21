namespace MyCv.Domain.Entities;

public class Experience
{
    public Guid Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string RoleFr { get; set; } = string.Empty;
    public string? CompanyUrl { get; set; }
    public string? Location { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsCurrentPosition { get; set; }
    public string Description { get; set; } = string.Empty;
    public string DescriptionFr { get; set; } = string.Empty;
    public string[] TechStack { get; set; } = [];
    public string[] Highlights { get; set; } = [];
    public string[] HighlightsFr { get; set; } = [];
    public int SortOrder { get; set; }
}
