using System.Text;
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
}