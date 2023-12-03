using Krosoft.Extensions.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Krosoft.Extensions.Testing.WebApi;

public abstract class ControllerBaseTest<T> : BaseTest where T : ApiControllerBase, new()
{
    protected T Controller = null!;
    protected Mock<IMediator> Mock = null!;

    [TestInitialize]
    public void SetUp()
    {
        Mock = new Mock<IMediator>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(x => x.GetService(typeof(IMediator)))
            .Returns(Mock.Object);

        Controller = new T

        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { RequestServices = serviceProviderMock.Object }
            }
        };
    }
}