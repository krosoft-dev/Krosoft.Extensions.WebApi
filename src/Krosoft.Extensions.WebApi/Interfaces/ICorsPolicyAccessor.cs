using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Krosoft.Extensions.WebApi.Interfaces;

public interface ICorsPolicyAccessor

{
    CorsPolicy? GetPolicy();

    CorsPolicy? GetPolicy(string name);
}