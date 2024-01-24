namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IJwtTokenValidator
{
    string? GetIdentifierFromToken(string accessToken);
}