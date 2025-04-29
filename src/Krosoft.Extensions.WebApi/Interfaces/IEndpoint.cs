#if NET9_0_OR_GREATER
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
#endif

namespace Krosoft.Extensions.WebApi.Interfaces;

public interface IEndpoint
{
#if NET9_0_OR_GREATER
    void Register(RouteGroupBuilder group);
    RouteGroupBuilder DefineGroup(WebApplication app);
#endif
}
