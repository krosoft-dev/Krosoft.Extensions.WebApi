using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();
}