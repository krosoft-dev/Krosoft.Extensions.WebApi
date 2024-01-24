using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.WebApi.Identity.Services;

public class HttpIdentifierProvider : IIdentifierProvider
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly IJwtTokenValidator _jwtTokenValidator;

    public HttpIdentifierProvider(IAccessTokenProvider accessTokenProvider, IJwtTokenValidator jwtTokenValidator)
    {
        _accessTokenProvider = accessTokenProvider;
        _jwtTokenValidator = jwtTokenValidator;
    }

    public async Task<string?> GetIdentifierAsync(CancellationToken cancellationToken)
    {
        var accessToken = await _accessTokenProvider.GetAccessTokenAsync(cancellationToken);
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new KrosoftTechniqueException("Impossible d'obtenir le token d'accès !");
        }

        var identifier = _jwtTokenValidator.GetIdentifierFromToken(accessToken);
        return identifier;
    }
}