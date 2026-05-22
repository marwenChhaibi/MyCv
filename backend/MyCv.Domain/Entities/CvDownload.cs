namespace MyCv.Domain.Entities;

public class CvDownload
{
    public int Id { get; set; }
    public DateTime DownloadedAt { get; set; } = DateTime.UtcNow;
    public string? UserAgent { get; set; }
    public string? FingerprintId { get; set; }
    public string? IpAddress { get; set; }
    public string? Country { get; set; }
    public string? Referrer { get; set; }
    public string? UtmSource { get; set; }
    public string? UtmMedium { get; set; }
    public string? DeviceType { get; set; }
    public string? Browser { get; set; }
    public string? Language { get; set; }
}
