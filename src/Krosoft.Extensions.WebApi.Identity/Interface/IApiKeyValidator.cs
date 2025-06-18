namespace Krosoft.Extensions.WebApi.Identity.Interface;

public interface IApiKeyValidator
{
    Task<bool> IsValidAsync(string? apiKey,
                            CancellationToken cancellationToken);
}