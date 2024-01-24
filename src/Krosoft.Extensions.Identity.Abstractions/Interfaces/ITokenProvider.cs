namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface ITokenProvider
{
    string GenerateToken(string purpose,
                         string securityStamp,
                         string identifier);

    bool Validate(string purpose,
                  string securityStamp,
                  string identifier,
                  string token);
}