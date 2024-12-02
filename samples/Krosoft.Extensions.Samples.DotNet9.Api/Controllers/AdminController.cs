using Krosoft.Extensions.Blocking.Abstractions.Interfaces;
using Krosoft.Extensions.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Controllers;

[AllowAnonymous]
public class AdminController : ApiControllerBase
{
    private readonly IAccessTokenBlockingService _accessTokenBlockingService;

    public AdminController(IAccessTokenBlockingService accessTokenBlockingService)
    {
        _accessTokenBlockingService = accessTokenBlockingService;
    }

    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [HttpGet("Blocked")]
    public Task<IEnumerable<string>> GetBlockedAsync(CancellationToken cancellationToken)
        => _accessTokenBlockingService.GetBlockedAsync(cancellationToken);

    [HttpPost("Block")]
    public Task GetBlockedAsync(string key, CancellationToken cancellationToken)
        => _accessTokenBlockingService.BlockAsync(key, cancellationToken);
}