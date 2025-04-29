using Krosoft.Extensions.Samples.DotNet9.Api.Modules;
using Krosoft.Extensions.WebApi.Identity.Extensions;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Extensions;

internal static class WebApplicationExtensions
{
    public static WebApplication AddModules(this WebApplication app)
    {
        var groupHello = app.MapGroup("/Hello").WithTags("Hello");

        groupHello.MapGet("/", HelloModule.Hello);
        groupHello.MapGet("/ApiKey", HelloModule.HelloApiKey).RequireApiKey();
        return app;
    }
}