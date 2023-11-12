using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Extensions;

public static class HttpClientExtensions
{
    public const string MediaTypeJson = "application/json";

    /// <summary>
    /// Default value for AuthenticationScheme property in the JwtBearerAuthenticationOptions
    /// </summary>
    public const string JwtAuthenticationScheme = "Bearer";

    public static async Task<T?> ReadAsJsonAsync<T>(this HttpContent content, CancellationToken cancellationToken = default)
    {
        var json = await content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static HttpClient SetBearerToken(this HttpClient client,
                                            string token)
    {
        client.SetToken(JwtAuthenticationScheme, token);

        return client;
    }

    public static HttpClient SetToken(this HttpClient client,
                                      string scheme,
                                      string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        return client;
    }

    public static async Task EnsureAsync(this Task<HttpResponseMessage> task,
                                         CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;

        await httpResponseMessage.EnsureAsync(cancellationToken);
    }

    public static async Task<T?> EnsureAsync<T>(this Task<HttpResponseMessage> task,
                                                CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;

        return await httpResponseMessage.EnsureAsync<T?>(cancellationToken);
    }

    public static Task<HttpResponseMessage> GetAsync<T>(this HttpClient httpClient, string requestUri, T data, CancellationToken cancellationToken)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri) { Content = Serialize(data) }, cancellationToken);

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data, CancellationToken cancellationToken)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

    public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data, CancellationToken cancellationToken)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

    private static HttpContent Serialize(object? data) => new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeJson);

    public static async Task<string?> EnsureStringAsync(this Task<HttpResponseMessage> task,
                                                        CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var template = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            return template;
        }

        await httpResponseMessage.ManageErrorAsync(cancellationToken);

        return null;
    }

    public static async Task<Stream?> EnsureStreamAsync(this Task<HttpResponseMessage> task,
                                                        CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await task;

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var stream = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);

            return stream;
        }

        await httpResponseMessage.ManageErrorAsync(cancellationToken);
        return null;
    }
}