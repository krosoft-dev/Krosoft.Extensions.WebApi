#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class HttpResultExtensions
{
    public static async Task<Ok> ToOkResult(this Task task)
    {
        await task;
        return TypedResults.Ok();
    }

    public static async Task<Ok<T>> ToOkResult<T>(this Task<T> task)
    {
        var value = await task;
        return TypedResults.Ok(value);
    }

    public static async Task<Created> ToCreatedResult(this Task task, string? uri = null)
    {
        await task;
        return TypedResults.Created(uri);
    }

    public static async Task<Created<T>> ToCreatedResult<T>(this Task<T> task, string? uri = null)
    {
        var value = await task;
        return TypedResults.Created(uri, value);
    }

    public static async Task<NoContent> ToNoContentResult(this Task task)
    {
        await task;
        return TypedResults.NoContent();
    }

    // Discard the handler return value — use when the operation produces data internally but the HTTP response should be 204.
    public static async Task<NoContent> ToNoContentResult<T>(this Task<T> task)
    {
        await task;
        return TypedResults.NoContent();
    }
}
#endif
