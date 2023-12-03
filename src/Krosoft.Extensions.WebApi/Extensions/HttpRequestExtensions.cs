using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class HttpRequestExtensions
{
    public static async Task<IEnumerable<string>> ToBase64StringAsync(this HttpRequest request)
    {
        var formCollection = await request.ReadFormAsync();
        var files = await formCollection.ToBase64StringAsync();
        return files;
    }
}