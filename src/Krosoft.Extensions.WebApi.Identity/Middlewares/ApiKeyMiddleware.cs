using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.Identity.Attributes;
using Krosoft.Extensions.WebApi.Identity.Interface;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Identity.Middlewares;

public class ApiKeyMiddleware
{
    public const string ApiKeyHeaderName = "x-api-key";

    private readonly IApiKeyValidator _apiKeyValidator;
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next, IApiKeyValidator apiKeyValidator)
    {
        _next = next;
        _apiKeyValidator = apiKeyValidator;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        var requiresApiKey = endpoint?.Metadata.GetMetadata<RequireApiKeyAttribute>() != null;

        if (!requiresApiKey)
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            await context.HandleExceptionAsync(new UnauthorizedException("Api Key was not provided."));
            return;
        }

        if (!_apiKeyValidator.IsValid(extractedApiKey))
        {
            await context.HandleExceptionAsync(new UnauthorizedException("Invalid Api Key provided."));
            return;
        }

        await _next(context);
    }
}