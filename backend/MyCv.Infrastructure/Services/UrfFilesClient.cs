using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace MyCv.Infrastructure.Services;

public class UrfFilesClient(HttpClient http)
{
    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent();
        using var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        form.Add(streamContent, "file", fileName);

        var response = await http.PostAsync("/api/files/upload", form, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UploadResult>(ct);
        return result!.Url;
    }

    private record UploadResult(string Url);
}
