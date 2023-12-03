using System.Net;
using System.Net.Mime;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.WebApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context,
                                             Exception ex)
    {
        var error = new ErrorDto
        {
            Status = HttpStatusCode.InternalServerError
        };

        if (!string.IsNullOrEmpty(ex.Message))
        {
            error.Errors.Add(ex.Message);
        }

        switch (ex)
        {
            case KrosoftMetierException validationException:
                error.Status = validationException.Code;
                error.Errors.AddRange(validationException.Erreurs);
                break;
            case KrosoftTechniqueException techniqueException:
                error.Status = techniqueException.Code;
                error.Errors.AddRange(techniqueException.Erreurs);
                break;
            case HttpException re:
                error.Status = re.Code;
                break;
        }

        var result = JsonConvert.SerializeObject(error);
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)error.Status;
        return context.Response.WriteAsync(result);
    }
}