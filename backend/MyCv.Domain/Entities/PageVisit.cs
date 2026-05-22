namespace MyCv.Domain.Entities;

public class PageVisit
{
    public int Id { get; set; }
    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
    public string? UserAgent { get; set; }
    public string? FingerprintId { get; set; }
    public string? IpAddress { get; set; }
    public string? Country { get; set; }
    public string? Referrer { get; set; }
    public string? DeviceType { get; set; }   // mobile / tablet / desktop
    public string? Browser { get; set; }      // Chrome / Firefox / Safari / Edge / Other
    public string? Language { get; set; }     // e.g. "fr", "en"
}
