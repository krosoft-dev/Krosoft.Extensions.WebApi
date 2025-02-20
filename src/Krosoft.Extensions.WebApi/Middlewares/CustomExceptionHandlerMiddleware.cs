using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.WebApi.Middlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logProvider;
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next,
                                            ILogger<CustomExceptionHandlerMiddleware> logProvider)
    {
        _next = next;
        _logProvider = logProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logProvider.LogError(ex, ex.Message);
            await context.HandleExceptionAsync(ex);
        }
    }
}