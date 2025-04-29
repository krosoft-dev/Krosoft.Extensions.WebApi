#if NET9_0_OR_GREATER
using System.Reflection;
using Krosoft.Extensions.WebApi.Interfaces;
using Microsoft.AspNetCore.Builder;
#endif

namespace Krosoft.Extensions.WebApi.Extensions;

public static class WebApplicationExtensions
{
#if NET9_0_OR_GREATER
    public static WebApplication AddEndpoints(this WebApplication app, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var endpointTypes = assembly
                                .GetTypes()
                                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(IEndpoint).IsAssignableFrom(t))
                                .ToList();

            foreach (var endpointType in endpointTypes)
            {
                // Instanciation de l'endpoint
                if (Activator.CreateInstance(endpointType) is IEndpoint endpoint)
                {
                    // Création du groupe de routes
                    var group = endpoint.DefineGroup(app);

                    // Enregistrement des endpoints dans le groupe
                    endpoint.Register(group);
                }
            }
        }

        return app;
    }
#endif
}