using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Modo.Clients;

public static class HttpExtensions
{
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        Converters = { new StringEnumConverter() }
    };

    public static async Task<T?> ReadFromJsonAsync<T>(this HttpContent content, CancellationToken cancellationToken = default)
    {
        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        var str = await content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<T>(str, Settings);
    }

    public static async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, string? requestUri,
        TValue value, CancellationToken cancellationToken = default)
    {
        var content = new StringContent(JsonConvert.SerializeObject(value, Settings), Encoding.UTF8);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return await client.PostAsync(requestUri, content, cancellationToken);
    }

    public static async Task<HttpResponseMessage> PutAsJsonAsync<TValue>(this HttpClient client, string? requestUri,
        TValue value, CancellationToken cancellationToken = default)
    {
        var content = new StringContent(JsonConvert.SerializeObject(value, Settings), Encoding.UTF8);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return await client.PutAsync(requestUri, content, cancellationToken);
    }
    
    public static async Task<HttpResponseMessage> PathAsJsonAsync<TValue>(this HttpClient client, string? requestUri,
        TValue value, CancellationToken cancellationToken = default)
    {
        var content = new StringContent(JsonConvert.SerializeObject(value, Settings), Encoding.UTF8);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return await client.PatchAsync(requestUri, content, cancellationToken);
    }
}