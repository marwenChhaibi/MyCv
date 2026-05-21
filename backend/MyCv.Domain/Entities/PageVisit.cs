namespace MyCv.Domain.Entities;

public class PageVisit
{
    public int Id { get; set; }
    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
    public string? UserAgent { get; set; }
}
