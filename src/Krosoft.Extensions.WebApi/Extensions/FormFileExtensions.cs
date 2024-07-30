using System.Text;
using Krosoft.Extensions.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class FormFileExtensions
{
    public static async Task<byte[]> GetBytes(this IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        using var reader = new StreamReader(memoryStream, Encoding.UTF8, true);
        await formFile.CopyToAsync(reader.BaseStream);
        return memoryStream.ToArray();
    }
 
    public static async Task<KrosoftFile?> ToFileAsync(this IFormFile? formFile, CancellationToken cancellationToken)
    {
        if (formFile == null)
        {
            return null;
        }

        using var memoryStream = new MemoryStream();
        using var reader = new StreamReader(memoryStream, Encoding.UTF8, true);
        await formFile.CopyToAsync(reader.BaseStream, cancellationToken);

        return new KrosoftFile(formFile.FileName, memoryStream.ToArray());
    }
}