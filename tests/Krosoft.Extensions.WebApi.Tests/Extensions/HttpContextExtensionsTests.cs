using System.Net;
using System.Net.Mime;
using System.Text;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Krosoft.Extensions.WebApi.Tests.Extensions;

[TestClass]
public class HttpContextExtensionsTests
{
    [TestMethod]
    public async Task HandleExceptionAsync_WithGenericException_SetsInternalServerError()
    {
        var exception = new Exception("Test exception");
        var context = CreateMockHttpContext();

        await context.HandleExceptionAsync(exception);

        var responseBody = GetResponseBody(context);
        var errorDto = JsonConvert.DeserializeObject<ErrorDto>(responseBody);

        Check.That(context.Response.StatusCode).IsEqualTo((int)HttpStatusCode.InternalServerError);
        Check.That(context.Response.ContentType).IsEqualTo(MediaTypeNames.Application.Json);
        Check.That(errorDto).IsNotNull();
        Check.That(errorDto!.Code).IsEqualTo((int)HttpStatusCode.InternalServerError);
        Check.That(errorDto.Message).IsEqualTo(nameof(HttpStatusCode.InternalServerError));
        Check.That(errorDto.Errors).ContainsExactly("Test exception");
    }

    [TestMethod]
    public async Task HandleExceptionAsync_WithKrosoftFunctionalException_SetsCorrectStatusCode()
    {
        var errors = new[] { "Error1", "Error2" };
        var exception = new KrosoftFunctionalException("Test", errors.ToHashSet());
        var context = CreateMockHttpContext();

        await context.HandleExceptionAsync(exception);

        var responseBody = GetResponseBody(context);
        var errorDto = JsonConvert.DeserializeObject<ErrorDto>(responseBody);

        Check.That(context.Response.StatusCode).IsEqualTo((int)HttpStatusCode.BadRequest);
        Check.That(context.Response.ContentType).IsEqualTo(MediaTypeNames.Application.Json);
        Check.That(errorDto).IsNotNull();
        Check.That(errorDto!.Code).IsEqualTo((int)HttpStatusCode.BadRequest);
        Check.That(errorDto.Message).IsEqualTo(nameof(HttpStatusCode.BadRequest));
        Check.That(errorDto.Errors).ContainsExactly("Test","Error1","Error2");
    }

    [TestMethod]
    public async Task HandleExceptionAsync_WithKrosoftTechnicalException_SetsCorrectStatusCode()
    {
        var errors = new[] { "TechError1", "TechError2" };
        var exception = new KrosoftTechnicalException(errors.ToHashSet());
        var context = CreateMockHttpContext();

        await context.HandleExceptionAsync(exception);

        var responseBody = GetResponseBody(context);
        var errorDto = JsonConvert.DeserializeObject<ErrorDto>(responseBody);

        Check.That(context.Response.StatusCode).IsEqualTo((int)HttpStatusCode.InternalServerError);
        Check.That(context.Response.ContentType).IsEqualTo(MediaTypeNames.Application.Json);
        Check.That(errorDto).IsNotNull();
        Check.That(errorDto!.Code).IsEqualTo((int)HttpStatusCode.InternalServerError);
        Check.That(errorDto.Message).IsEqualTo(nameof(HttpStatusCode.InternalServerError));
        Check.That(errorDto.Errors).ContainsExactly("TechError1", "TechError2");
    }

    [TestMethod]
    public async Task HandleExceptionAsync_WithHttpException_SetsCorrectStatusCode()
    {
        var exception = new HttpException(HttpStatusCode.NotFound, "TEST");
        var context = CreateMockHttpContext();

        await context.HandleExceptionAsync(exception);

        var responseBody = GetResponseBody(context);
        var errorDto = JsonConvert.DeserializeObject<ErrorDto>(responseBody);

        Check.That(context.Response.StatusCode).IsEqualTo((int)HttpStatusCode.NotFound);
        Check.That(context.Response.ContentType).IsEqualTo(MediaTypeNames.Application.Json);
        Check.That(errorDto).IsNotNull();
        Check.That(errorDto!.Code).IsEqualTo((int)HttpStatusCode.NotFound);
        Check.That(errorDto.Message).IsEqualTo("NotFound");
        Check.That(errorDto.Errors).ContainsExactly("TEST");
    }

    [TestMethod]
    public async Task HandleExceptionAsync_WithEmptyExceptionMessage_NoKrosoft()
    {
        var exception = new Exception();
        var context = CreateMockHttpContext();

        await context.HandleExceptionAsync(exception);

        var responseBody = GetResponseBody(context);
        var errorDto = JsonConvert.DeserializeObject<ErrorDto>(responseBody);

        Check.That(context.Response.StatusCode).IsEqualTo((int)HttpStatusCode.InternalServerError);
        Check.That(context.Response.ContentType).IsEqualTo(MediaTypeNames.Application.Json);
        Check.That(errorDto).IsNotNull();
        Check.That(errorDto!.Code).IsEqualTo((int)HttpStatusCode.InternalServerError);
        Check.That(errorDto.Message).IsEqualTo(nameof(HttpStatusCode.InternalServerError)); 
        Check.That(errorDto.Errors).ContainsExactly("Exception of type 'System.Exception' was thrown.");
    }

    private static HttpContext CreateMockHttpContext()
    {
        var context = new DefaultHttpContext();
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;
        return context;
    }

    private static string GetResponseBody(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body, Encoding.UTF8);
        return reader.ReadToEnd();
    }
}