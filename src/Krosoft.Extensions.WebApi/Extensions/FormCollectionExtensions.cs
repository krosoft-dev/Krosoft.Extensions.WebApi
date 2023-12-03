using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class FormCollectionExtensions
{
    public static async Task<IEnumerable<string>> ToBase64StringAsync(this IFormCollection formCollection)
    {
        var files = new List<string>();

        foreach (var file in formCollection.Files)
        {
            var bytes = await file.GetBytes();
            var hexString = Convert.ToBase64String(bytes);
            files.Add(hexString);
        }

        return files;
    }
}