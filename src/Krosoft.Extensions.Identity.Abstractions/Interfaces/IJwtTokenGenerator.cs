using System.Security.Claims;

namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IJwtTokenGenerator
{
    string CreateToken(string identifier, IEnumerable<Claim> claims);
}