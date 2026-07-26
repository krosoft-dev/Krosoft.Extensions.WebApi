using Microsoft.AspNetCore.Http.HttpResults;
using Krosoft.Extensions.WebApi.Extensions;

namespace Krosoft.Extensions.WebApi.Tests.Extensions;

[TestClass]
public class HttpResultExtensionsTests
{
    [TestMethod]
    public async Task ToOkResult_OnVoidTask_ReturnsOk()
    {
        var result = await Task.CompletedTask.ToOkResult();

        Check.That(result).IsInstanceOf<Ok>();
        Check.That(result.StatusCode).IsEqualTo(200);
    }

    [TestMethod]
    public async Task ToOkResult_ReturnsOkWithValue()
    {
        var result = await Task.FromResult("hello").ToOkResult();

        Check.That(result).IsInstanceOf<Ok<string>>();
        Check.That(result.Value).IsEqualTo("hello");
        Check.That(result.StatusCode).IsEqualTo(200);
    }

    [TestMethod]
    public async Task ToOkResult_WithPropertyName_ReturnsOkWithSinglePropertyObject()
    {
        var result = await Task.FromResult("https://auth.example/authorize").ToOkResult("authorizationUrl");

        Check.That(result).IsInstanceOf<Ok<Dictionary<string, string>>>();
        Check.That(result.StatusCode).IsEqualTo(200);
        Check.That(result.Value).IsNotNull();
        Check.That(result.Value!).ContainsKey("authorizationUrl");
        Check.That(result.Value!["authorizationUrl"]).IsEqualTo("https://auth.example/authorize");
    }

    [TestMethod]
    public async Task ToCreatedResult_OnVoidTask_WithoutUri_ReturnsCreated()
    {
        var result = await Task.CompletedTask.ToCreatedResult();

        Check.That(result).IsInstanceOf<Created>();
        Check.That(result.StatusCode).IsEqualTo(201);
    }

    [TestMethod]
    public async Task ToCreatedResult_OnVoidTask_WithUri_ReturnsCreatedWithLocation()
    {
        var result = await Task.CompletedTask.ToCreatedResult("/resource/1");

        Check.That(result).IsInstanceOf<Created>();
        Check.That(result.StatusCode).IsEqualTo(201);
        Check.That(result.Location).IsEqualTo("/resource/1");
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
    public async Task ToAcceptedResult_OnVoidTask_WithoutUri_ReturnsAccepted()
    {
        var result = await Task.CompletedTask.ToAcceptedResult();

        Check.That(result).IsInstanceOf<Accepted>();
        Check.That(result.StatusCode).IsEqualTo(202);
        Check.That(result.Location).IsNull();
    }

    [TestMethod]
    public async Task ToAcceptedResult_OnVoidTask_WithUri_ReturnsAcceptedWithLocation()
    {
        var result = await Task.CompletedTask.ToAcceptedResult("/jobs/1");

        Check.That(result).IsInstanceOf<Accepted>();
        Check.That(result.StatusCode).IsEqualTo(202);
        Check.That(result.Location).IsEqualTo("/jobs/1");
    }

    [TestMethod]
    public async Task ToAcceptedResult_WithoutUri_ReturnsAcceptedWithValue()
    {
        var result = await Task.FromResult("hello").ToAcceptedResult();

        Check.That(result).IsInstanceOf<Accepted<string>>();
        Check.That(result.Value).IsEqualTo("hello");
        Check.That(result.StatusCode).IsEqualTo(202);
        Check.That(result.Location).IsNull();
    }

    [TestMethod]
    public async Task ToAcceptedResult_WithUri_ReturnsAcceptedWithLocation()
    {
        var result = await Task.FromResult("hello").ToAcceptedResult("/jobs/1");

        Check.That(result).IsInstanceOf<Accepted<string>>();
        Check.That(result.Value).IsEqualTo("hello");
        Check.That(result.StatusCode).IsEqualTo(202);
        Check.That(result.Location).IsEqualTo("/jobs/1");
    }

    [TestMethod]
    public void ToAcceptedResult_WhenTaskFails_PropagatesException()
    {
        Check.ThatCode(async () => await Task.FromException(new InvalidOperationException("boom")).ToAcceptedResult())
             .Throws<InvalidOperationException>()
             .WithMessage("boom");
    }

    [TestMethod]
    public async Task ToRedirectResult_ReturnsRedirectWithUrl()
    {
        var result = await Task.FromResult("https://app.example/callback?ok=1").ToRedirectResult();

        Check.That(result).IsInstanceOf<RedirectHttpResult>();
        Check.That(result.Url).IsEqualTo("https://app.example/callback?ok=1");
        Check.That(result.Permanent).IsFalse();
        Check.That(result.PreserveMethod).IsFalse();
    }

    [TestMethod]
    public async Task ToRedirectResult_WithPermanentAndPreserveMethod_ReturnsRedirect()
    {
        var result = await Task.FromResult("/home").ToRedirectResult(true, true);

        Check.That(result.Url).IsEqualTo("/home");
        Check.That(result.Permanent).IsTrue();
        Check.That(result.PreserveMethod).IsTrue();
    }

    [TestMethod]
    public async Task ToRedirectResult_WithPermanentOnly_ReturnsRedirect()
    {
        var result = await Task.FromResult("/home").ToRedirectResult(true);

        Check.That(result.Permanent).IsTrue();
        Check.That(result.PreserveMethod).IsFalse();
    }

    [TestMethod]
    public async Task ToRedirectResult_WithPreserveMethodOnly_ReturnsRedirect()
    {
        var result = await Task.FromResult("/home").ToRedirectResult(false, true);

        Check.That(result.Permanent).IsFalse();
        Check.That(result.PreserveMethod).IsTrue();
    }

    [TestMethod]
    public void ToRedirectResult_WhenUrlIsNull_Throws()
    {
        Check.ThatCode(async () => await Task.FromResult<string>(null!).ToRedirectResult())
             .Throws<ArgumentNullException>();
    }

    [TestMethod]
    public void ToRedirectResult_WhenUrlIsEmpty_Throws()
    {
        Check.ThatCode(async () => await Task.FromResult(string.Empty).ToRedirectResult())
             .Throws<ArgumentException>();
    }

    [TestMethod]
    public void ToRedirectResult_WhenTaskFails_PropagatesException()
    {
        Check.ThatCode(async () => await Task.FromException<string>(new InvalidOperationException("boom")).ToRedirectResult())
             .Throws<InvalidOperationException>()
             .WithMessage("boom");
    }

    [TestMethod]
    public async Task ToRedirectResult_OnUri_ReturnsRedirectWithUrl()
    {
        var result = await Task.FromResult(new Uri("https://app.example/callback")).ToRedirectResult();

        Check.That(result).IsInstanceOf<RedirectHttpResult>();
        Check.That(result.Url).IsEqualTo("https://app.example/callback");
    }

    [TestMethod]
    public async Task ToRedirectResult_OnRelativeUri_ReturnsRedirectWithUrl()
    {
        var result = await Task.FromResult(new Uri("/callback?ok=1", UriKind.Relative)).ToRedirectResult();

        Check.That(result.Url).IsEqualTo("/callback?ok=1");
    }

    [TestMethod]
    public async Task ToRedirectResult_OnUri_WithPermanentAndPreserveMethod_ReturnsRedirect()
    {
        var result = await Task.FromResult(new Uri("https://app.example/callback")).ToRedirectResult(true, true);

        Check.That(result.Permanent).IsTrue();
        Check.That(result.PreserveMethod).IsTrue();
    }

    [TestMethod]
    public void ToRedirectResult_OnUri_WhenUriIsNull_Throws()
    {
        Check.ThatCode(async () => await Task.FromResult<Uri>(null!).ToRedirectResult())
             .Throws<ArgumentNullException>();
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
