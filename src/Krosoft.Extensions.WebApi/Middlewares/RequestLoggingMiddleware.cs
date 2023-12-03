using System.Diagnostics;
using Krosoft.Extensions.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.WebApi.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logProvider;
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logProvider)
        {
            _next = next;
            _logProvider = logProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTime.Now;

            var sw = Stopwatch.StartNew();
            await _next.Invoke(context);

            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            var requestPath = context.Request.Path;
            var requestContentType = context.Request.ContentType;
            var requestContentLength = context.Request.ContentLength;

            var logTemplate =
                $"Client IP: {clientIp}, Request path: {requestPath}, Request content type: {requestContentType}, Request content length: {requestContentLength}, Start time: {startTime}, Duration: {sw.Elapsed.ToShortString()}";

            _logProvider.LogInformation(logTemplate);
        }
    }
}