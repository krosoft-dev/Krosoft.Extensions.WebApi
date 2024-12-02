using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Controllers;

[AllowAnonymous]
public class HelloController : ApiControllerBase
{
    [HttpGet]
    public Task<string> HelloAsync(CancellationToken cancellationToken)
        => Mediator.Send(new HelloDotNet9Query(), cancellationToken);
}