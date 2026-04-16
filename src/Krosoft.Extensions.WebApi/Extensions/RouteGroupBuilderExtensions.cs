#if NET9_0_OR_GREATER
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
#endif

namespace Krosoft.Extensions.WebApi.Extensions;

public static class RouteGroupBuilderExtensions
{
#if NET9_0_OR_GREATER
    public static RouteGroupBuilder MapTaggedGroup(this WebApplication app,
                                                   [StringSyntax("Route")] string pattern,
                                                   string? tag = null)
    {
        var derivedTag = tag ?? Regex.Replace(pattern.TrimStart('/'), @"/\{[^}]+\}", "", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        return app.MapGroup(pattern).WithTags(derivedTag);
    }

#endif
}