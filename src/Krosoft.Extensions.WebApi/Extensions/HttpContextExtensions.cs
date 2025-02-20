using System.Net;
using System.Net.Mime;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class HttpContextExtensions
{
    public static Task HandleExceptionAsync(this HttpContext context,
                                            Exception ex)
    {
        var status = HttpStatusCode.InternalServerError;
        var errors = new HashSet<string>();

        if (!string.IsNullOrEmpty(ex.Message))
        {
            errors.Add(ex.Message);
        }

        switch (ex)
        {
            case KrosoftFunctionalException validationException:
                status = validationException.Code;
                errors.AddRange(validationException.Errors);
                break;
            case KrosoftTechnicalException techniqueException:
                status = techniqueException.Code;
                errors.AddRange(techniqueException.Errors);
                break;
            case HttpException re:
                status = re.Code;
                break;
        }

        var error = new ErrorDto
        {
            Code = (int)status,
            Message = status.ToString(),
            Errors = errors
        };
        var result = JsonConvert.SerializeObject(error);
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = error.Code;
        return context.Response.WriteAsync(result);
    }
}