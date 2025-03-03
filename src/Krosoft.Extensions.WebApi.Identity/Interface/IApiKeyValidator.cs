namespace Krosoft.Extensions.WebApi.Identity.Interface;

public interface IApiKeyValidator
{
    bool IsValid(string? apiKey);

    Task<bool> IsValidAsync(string? apiKey,
                            CancellationToken cancellationToken);
}

