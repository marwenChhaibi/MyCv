using MyCv.Application.Common;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MyCv.Infrastructure.Services;

public class GeoLocationService(HttpClient http) : IGeoLocationService
{
    public async Task<string?> GetCountryAsync(string ip, CancellationToken ct = default)
    {
        try
        {
            if (IsPrivateOrLoopback(ip)) return null;

            var response = await http.GetFromJsonAsync<IpApiResponse>(
                $"http://ip-api.com/json/{ip}?fields=status,country",
                ct);

            return response?.Status == "success" ? response.Country : null;
        }
        catch
        {
            return null;
        }
    }

    private static bool IsPrivateOrLoopback(string ip)
    {
        if (!IPAddress.TryParse(ip, out var addr)) return true;
        if (IPAddress.IsLoopback(addr)) return true;

        var bytes = addr.GetAddressBytes();
        if (bytes.Length == 4)
        {
            return bytes[0] == 10
                || (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
                || (bytes[0] == 192 && bytes[1] == 168);
        }
        return false;
    }

    private record IpApiResponse(
        [property: JsonPropertyName("status")] string? Status,
        [property: JsonPropertyName("country")] string? Country);
}
