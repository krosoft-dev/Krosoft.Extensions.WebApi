using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.WebApi.Controllers;
using Krosoft.Extensions.WebApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Controllers;

[AllowAnonymous]
public class PaysController : ApiControllerBase
{
    [ProducesResponseType(typeof(IEnumerable<PaysDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public Task<IEnumerable<PaysDto>> GetAsync([FromQuery] PaysQuery query,
                                               CancellationToken cancellationToken)
        => Mediator.Send(query, cancellationToken);
}