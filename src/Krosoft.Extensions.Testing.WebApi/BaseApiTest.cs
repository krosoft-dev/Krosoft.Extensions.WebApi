using System.Net;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Testing.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

//using Krosoft.Extensions.Cache.Distributed.Redis.Interfaces;
//using Krosoft.Extensions.Core.Models.Business;
//using Krosoft.Extensions.Data.EntityFramework.Contexts;
//using Krosoft.Extensions.Testing.Extensions;

namespace Krosoft.Extensions.Testing.WebApi;

public abstract class BaseApiTest<TStartup, TKrosoftContext> : BaseTest
    where TStartup : class
//where TKrosoftContext : KrosoftContext
{
    protected CustomWebApplicationFactory<TStartup, TKrosoftContext> Factory = null!;
    protected virtual bool UseFakeAuth => true;

    protected virtual void ConfigureClaims(KrosoftToken krosoftToken)
    {
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }

    protected CustomWebApplicationFactory<TStartup, TKrosoftContext> GetFactory()
    {
        void Action(IServiceCollection services)
        {
            ConfigureServices(services);

            //Mock pour HttpClient.
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                                  .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                                  .ReturnsAsync(new HttpResponseMessage
                                  {
                                      StatusCode = HttpStatusCode.OK
                                  });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);
            services.SwapTransient(_ => mockHttpClientFactory.Object);
        }

        return new CustomWebApplicationFactory<TStartup, TKrosoftContext>(Action, null, UseFakeAuth);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        Factory.Dispose();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        Factory = new CustomWebApplicationFactory<TStartup, TKrosoftContext>(ConfigureServices, ConfigureClaims, UseFakeAuth);
    }
}