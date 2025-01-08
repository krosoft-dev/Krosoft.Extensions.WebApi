using Krosoft.Extensions.Samples.DotNet9.Api.Modules;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Extensions;

internal static class WebApplicationExtensions
{
    public static WebApplication AddEndpoints(this WebApplication app)
    {
        var groupHello = app.MapGroup("/Hello")
            //.WithTags("Documents")
            ;

        groupHello.MapGet("/", HelloModule.Hello);
        return app;
    }
}