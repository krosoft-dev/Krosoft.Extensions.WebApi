using Krosoft.Extensions.WebApi.Identity.Interface;
using Krosoft.Extensions.WebApi.Identity.Models;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.WebApi.Identity.Services;

internal class SettingsApiKeyValidator : IApiKeyValidator
{
    private readonly IOptions<WebApiIdentySettings> _options;

    public SettingsApiKeyValidator(IOptions<WebApiIdentySettings> options)
    {
        _options = options;
    }

    public bool IsValid(string? apiKey)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            return false;
        }

        var keySettings = _options.Value.Keys.FirstOrDefault(x => x.Key == apiKey);
        if (keySettings == null)
        {
            return false;
        }

        return true;
    }

    public Task<bool> IsValidAsync(string? apiKey,
                                   CancellationToken cancellationToken)
        => Task.FromResult(IsValid(apiKey));
}