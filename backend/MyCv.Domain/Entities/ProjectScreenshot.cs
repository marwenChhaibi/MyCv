namespace MyCv.Domain.Entities;

public class ProjectScreenshot
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public string? CaptionFr { get; set; }
    public int SortOrder { get; set; }
    public Project Project { get; set; } = null!;
}
