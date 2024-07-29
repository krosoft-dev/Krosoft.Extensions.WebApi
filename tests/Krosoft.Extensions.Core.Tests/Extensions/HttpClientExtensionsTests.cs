using System.Net;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.Samples.Library.Models;
using Krosoft.Extensions.Testing;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class HttpClientExtensionsTests : BaseTest
{
    [TestMethod]
    public async Task EnsureAsync()
    {
        var cancellationToken = CancellationToken.None;

        var result = await new HttpClient()
                           .GetAsync("https://jsonplaceholder.typicode.com/todos", cancellationToken)
                           .EnsureAsync<IEnumerable<TodoHttp>>((_, json) => throw new KrosoftTechnicalException(json), cancellationToken);

        Check.That(result).IsNotNull();
        Check.That(result).HasSize(200);
    }

    [TestMethod]
    public void EnsureAsync_Exception_Custom()
    {
        var cancellationToken = CancellationToken.None;

        Check.ThatCode(() => new HttpClient()
                             .PutAsync("https://jsonplaceholder.typicode.com/posts", null, cancellationToken)
                             .EnsureAsync<IEnumerable<TodoHttp>>((code, json) => throw new HttpException(HttpStatusCode.InternalServerError, json), cancellationToken))
             .Throws<HttpException>()
             .WithMessage("{}")
             .And.WhichMember(x => x.Code)
             .IsEqualTo(HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public void EnsureAsync_Exception_Default()
    {
        var cancellationToken = CancellationToken.None;

        Check.ThatCode(() => new HttpClient()
                             .PutAsync("https://jsonplaceholder.typicode.com/posts", null, cancellationToken)
                             .EnsureAsync<IEnumerable<TodoHttp>>(cancellationToken))
             .Throws<HttpException>()
             .WithMessage("Not Found")
             .And.WhichMember(x => x.Code)
             .IsEqualTo(HttpStatusCode.NotFound);
    }
}