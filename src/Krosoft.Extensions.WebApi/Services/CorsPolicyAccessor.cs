using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.WebApi.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.WebApi.Services;

public class CorsPolicyAccessor : ICorsPolicyAccessor
{
    private readonly CorsOptions _options;

    public CorsPolicyAccessor(IOptions<CorsOptions> options)
    {
        Guard.IsNotNull(nameof(options), options);

        _options = options.Value;
    }

    public CorsPolicy? GetPolicy() => _options.GetPolicy(_options.DefaultPolicyName);

    public CorsPolicy? GetPolicy(string name) => _options.GetPolicy(name);
}