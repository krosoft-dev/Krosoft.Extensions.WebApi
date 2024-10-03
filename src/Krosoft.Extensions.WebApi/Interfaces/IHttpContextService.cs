namespace Krosoft.Extensions.WebApi.Interfaces;

public interface IHttpContextService
{
    string GetBaseUrl();
    IEnumerable<string> GetInformations();
}