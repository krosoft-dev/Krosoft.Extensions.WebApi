using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.WebApi.Identity.Models;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.WebApi.Identity.Services;

internal class SettingsApiKeyStorageProvider : IApiKeyStorageProvider
{
    private readonly IOptions<WebApiIdentySettings> _options;

    public SettingsApiKeyStorageProvider(IOptions<WebApiIdentySettings> options)
    {
        _options = options;
    }

    public Task<string?> GetIdentifiantAsync(string? key, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(key))
        {
            return Task.FromResult<string?>(null);
        }

        var keySettings = _options.Value.Keys.FirstOrDefault(x => x.Key == key);
        if (keySettings == null)
        {
            return Task.FromResult<string?>(null);
        }

        return Task.FromResult(keySettings.Identifiant);
    }

    public Task<string?> GetKeyAsync(string? identifiant, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(identifiant))
        {
            return Task.FromResult<string?>(null);
        }

        var keySettings = _options.Value.Keys.FirstOrDefault(x => x.Identifiant == identifiant);
        if (keySettings == null)
        {
            return Task.FromResult<string?>(null);
        }

        return Task.FromResult(keySettings.Key);
    }
}