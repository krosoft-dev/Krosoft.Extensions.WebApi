using Krosoft.Extensions.Samples.DotNet6.Api.Models.Queries;
using Krosoft.Extensions.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Controllers;

public class HelloController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public Task<string> HelloWorldAsync(CancellationToken cancellationToken)
        => Mediator.Send(new HelloWorlQuery(), cancellationToken);
}