namespace MyCv.Domain.Entities;

public class ProjectFeature
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Label { get; set; } = string.Empty;
    public string LabelFr { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public Project Project { get; set; } = null!;
}
