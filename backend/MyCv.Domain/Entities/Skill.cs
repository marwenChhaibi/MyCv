namespace MyCv.Domain.Entities;

public class Skill
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string CategoryFr { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public SkillLevel Level { get; set; } = SkillLevel.Advanced;
    public int SortOrder { get; set; }
}

public enum SkillLevel
{
    Familiar,
    Intermediate,
    Advanced,
    Expert
}
