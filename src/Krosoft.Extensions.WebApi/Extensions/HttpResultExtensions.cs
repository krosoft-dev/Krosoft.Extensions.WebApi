#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class HttpResultExtensions
{
    public static async Task<Ok<T>> ToOkResult<T>(this Task<T> task)
    {
        var value = await task;
        return TypedResults.Ok(value);
    }

    public static async Task<Created<T>> ToCreatedResult<T>(this Task<T> task, string? uri = null)
    {
        var value = await task;
#if NET7_0
        // TypedResults.Created(string, T) is non-nullable on net7 — pass empty string when no URI is provided.
        return TypedResults.Created(uri ?? string.Empty, value);
#else
        return TypedResults.Created(uri, value);
#endif
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
