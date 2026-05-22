namespace MyCv.Application.Common;

public interface IGeoLocationService
{
    Task<string?> GetCountryAsync(string ip, CancellationToken ct = default);
}
