using Krosoft.Extensions.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class FileStreamExtensions
{
    public static FileStreamResult ToFileStreamResult(this IFileStream file)
    {
        var fileStreamResult = new FileStreamResult(file.Stream, file.ContentType) { FileDownloadName = file.FileName };
        return fileStreamResult;
    }

    public static async Task<FileStreamResult> ToFileStreamResult<T>(this Task<T> task) where T : IFileStream
    {
        var file = await task;
        return file.ToFileStreamResult();
    }
}