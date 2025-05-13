using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class FileStreamExtensions
{
    public static FileStreamResult ToFileStreamResult(this IFileStream file)
    {
        Guard.IsNotNull(nameof(file), file);
        Guard.IsNotNullOrWhiteSpace(nameof(file.FileName), file.FileName);
        Guard.IsNotNullOrWhiteSpace(nameof(file.ContentType), file.ContentType);

        var fileStreamResult = new FileStreamResult(file.Stream, file.ContentType) { FileDownloadName = file.FileName };
        return fileStreamResult;
    }

    public static async Task<FileStreamResult> ToFileStreamResult<T>(this Task<T> task) where T : IFileStream
    {
        var file = await task;
        return file.ToFileStreamResult();
    }
 
    public static IResult ToFileResult(this IFileStream file)
    {
        Guard.IsNotNull(nameof(file), file);
        Guard.IsNotNullOrWhiteSpace(nameof(file.FileName), file.FileName);
        Guard.IsNotNullOrWhiteSpace(nameof(file.ContentType), file.ContentType);
         

        return Results.File(file.Stream!, file.ContentType!, file.FileName!);
    }

    public static async Task<IResult> ToFileResult<T>(this Task<T> task) where T : IFileStream
    {
        var file = await task;
        return file.ToFileResult();
    }
}