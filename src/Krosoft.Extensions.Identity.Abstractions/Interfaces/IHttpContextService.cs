namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IHttpContextService
{
    Task<string> GetAccessTokenAsync();
}