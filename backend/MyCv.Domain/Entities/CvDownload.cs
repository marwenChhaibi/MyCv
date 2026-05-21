namespace MyCv.Domain.Entities;

public class CvDownload
{
    public int Id { get; set; }
    public DateTime DownloadedAt { get; set; } = DateTime.UtcNow;
    public string? UserAgent { get; set; }
}
