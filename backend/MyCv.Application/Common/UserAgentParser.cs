namespace MyCv.Application.Common;

public static class UserAgentParser
{
    public static string? ParseBrowser(string? ua)
    {
        if (string.IsNullOrEmpty(ua)) return null;
        if (ua.Contains("Edg/"))                          return "Edge";
        if (ua.Contains("OPR/") || ua.Contains("Opera/")) return "Opera";
        if (ua.Contains("Chrome/"))                        return "Chrome";
        if (ua.Contains("Firefox/"))                       return "Firefox";
        if (ua.Contains("Safari/"))                        return "Safari";
        return "Other";
    }
}
