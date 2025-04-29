using Krosoft.Extensions.Core.Models;
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
 
    public static async Task<KrosoftFile?> ToFileAsync(this HttpRequest request, string fileName, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await request.Body.CopyToAsync(memoryStream, cancellationToken);

        return new KrosoftFile(fileName, memoryStream.ToArray());
    }
}