using System.Net;

namespace Krosoft.Extensions.Core.Models.Exceptions.Http;

public class HttpException : KrosoftException
{
    public HttpException(HttpStatusCode code,
                         string? message) : base(message ?? string.Empty)
    {
        Code = code;
    }

    public HttpStatusCode Code { get; }
}