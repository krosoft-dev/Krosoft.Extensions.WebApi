using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.WebApi.Identity.Middlewares;
using Krosoft.Extensions.WebApi.Identity.Services;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Identity.Tests.Services;

[TestClass]
public class HttpApiKeyProviderTests
{
    private const string HeaderValue = "123456";

    [TestMethod]
    public async Task Should_Return_ApiKey_When_Header_Is_Present()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[ApiKeyMiddleware.ApiKeyHeaderName] = HeaderValue;

        var httpContextAccessor = new HttpContextAccessor { HttpContext = context };
        IApiKeyProvider provider = new HttpApiKeyProvider(httpContextAccessor);

        var result = await provider.GetApiKeyAsync(CancellationToken.None);

        Check.That(result).IsEqualTo(HeaderValue);
    }

    [TestMethod]
    public async Task Should_Return_Null_When_Header_Is_Missing()
    {
        var context = new DefaultHttpContext();
        var httpContextAccessor = new HttpContextAccessor { HttpContext = context };
        IApiKeyProvider provider = new HttpApiKeyProvider(httpContextAccessor);

        var result = await provider.GetApiKeyAsync(CancellationToken.None);

        Check.That(result).IsNull();
    }

    [TestMethod]
    public void Should_Throw_When_HttpContext_Is_Null()
    {
        var httpContextAccessor = new HttpContextAccessor { HttpContext = null };
        IApiKeyProvider provider = new HttpApiKeyProvider(httpContextAccessor);

        Check.ThatCode(async () => await provider.GetApiKeyAsync(CancellationToken.None))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("HttpContext non défini.");
    }
}