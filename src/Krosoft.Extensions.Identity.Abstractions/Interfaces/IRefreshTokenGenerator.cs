namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IRefreshTokenGenerator
{
    string CreateToken();
}