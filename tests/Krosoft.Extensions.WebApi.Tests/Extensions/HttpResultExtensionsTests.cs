using Microsoft.AspNetCore.Http.HttpResults;
using Krosoft.Extensions.WebApi.Extensions;

namespace Krosoft.Extensions.WebApi.Tests.Extensions;

[TestClass]
public class HttpResultExtensionsTests
{
    [TestMethod]
    public async Task ToOkResult_ReturnsOkWithValue()
    {
        var result = await Task.FromResult("hello").ToOkResult();

        Check.That(result).IsInstanceOf<Ok<string>>();
        Check.That(result.Value).IsEqualTo("hello");
        Check.That(result.StatusCode).IsEqualTo(200);
    }

    [TestMethod]
    public async Task ToCreatedResult_WithoutUri_ReturnsCreatedWithValue()
    {
        var result = await Task.FromResult("hello").ToCreatedResult();

        Check.That(result).IsInstanceOf<Created<string>>();
        Check.That(result.Value).IsEqualTo("hello");
        Check.That(result.StatusCode).IsEqualTo(201);
    }

    [TestMethod]
    public async Task ToCreatedResult_WithUri_ReturnsCreatedWithLocation()
    {
        var result = await Task.FromResult("hello").ToCreatedResult("/resource/1");

        Check.That(result).IsInstanceOf<Created<string>>();
        Check.That(result.Value).IsEqualTo("hello");
        Check.That(result.StatusCode).IsEqualTo(201);
        Check.That(result.Location).IsEqualTo("/resource/1");
    }

    [TestMethod]
    public async Task ToNoContentResult_OnVoidTask_ReturnsNoContent()
    {
        var result = await Task.CompletedTask.ToNoContentResult();

        Check.That(result).IsInstanceOf<NoContent>();
        Check.That(result.StatusCode).IsEqualTo(204);
    }

    [TestMethod]
    public async Task ToNoContentResult_WithValue_DiscardsValueReturnsNoContent()
    {
        var result = await Task.FromResult("discarded").ToNoContentResult();

        Check.That(result).IsInstanceOf<NoContent>();
        Check.That(result.StatusCode).IsEqualTo(204);
    }
}
