using Krosoft.Extensions.WebApi.Identity.Attributes;
using Krosoft.Extensions.WebApi.Identity.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Routing;

namespace Krosoft.Extensions.WebApi.Identity.Tests.Extensions;

[TestClass]
public class RouteGroupBuilderExtensionsTests
{
    private WebApplication _app = null!;

    [TestInitialize]
    public void SetUp()
    {
        var builder = WebApplication.CreateBuilder();
        _app = builder.Build();
    }

    [TestMethod]
    public void RequireApiKey_ShouldAddRequireApiKeyAttribute()
    {
        var group = _app.MapGroup("/test");

        var result = group.RequireApiKey();

        Check.That(result).IsSameReferenceAs(group);

        var endpoint = GetEndpoint(group, "/dummy-apikey");
        var attribute = endpoint.Metadata.GetMetadata<RequireApiKeyAttribute>();
        Check.That(attribute).IsNotNull();
    }

    [TestMethod]
    public void RequireAgentId_ShouldAddRequireAgentIdAttribute()
    {
        var group = _app.MapGroup("/test");

        var result = group.RequireAgentId();

        Check.That(result).IsSameReferenceAs(group);

        var endpoint = GetEndpoint(group, "/dummy-agentid");
        var attribute = endpoint.Metadata.GetMetadata<RequireAgentIdAttribute>();
        Check.That(attribute).IsNotNull();
    }

    [TestMethod]
    public void MapTaggedGroup_WithExplicitTag_ShouldUseProvidedTag()
    {
        var group = _app.MapTaggedGroup("/api/users", "custom-tag");

        var endpoint = GetEndpoint(group, "/dummy-tag1");
        var tags = endpoint.Metadata.GetMetadata<ITagsMetadata>();
        Check.That(tags).IsNotNull();
        Check.That(tags!.Tags).Contains("custom-tag");
    }

    [TestMethod]
    public void MapTaggedGroup_WithoutTag_ShouldDeriveTagFromPattern()
    {
        var group = _app.MapTaggedGroup("/api/users/{id}");

        var endpoint = GetEndpoint(group, "/dummy-tag2");
        var tags = endpoint.Metadata.GetMetadata<ITagsMetadata>();
        Check.That(tags).IsNotNull();
        Check.That(tags!.Tags).Contains("api/users");
    }

    [TestMethod]
    public void MapTaggedGroup_WithSimplePattern_ShouldDeriveTagFromPattern()
    {
        var group = _app.MapTaggedGroup("/products");

        var endpoint = GetEndpoint(group, "/dummy-tag3");
        var tags = endpoint.Metadata.GetMetadata<ITagsMetadata>();
        Check.That(tags).IsNotNull();
        Check.That(tags!.Tags).Contains("products");
    }

    [TestMethod]
    public void MapTaggedGroup_WithNestedPattern_ShouldDeriveTagFromPattern()
    {
        var group = _app.MapTaggedGroup("/api/orders/{orderId}/items");

        var endpoint = GetEndpoint(group, "/dummy-tag4");
        var tags = endpoint.Metadata.GetMetadata<ITagsMetadata>();
        Check.That(tags).IsNotNull();
        Check.That(tags!.Tags).Contains("api/orders/items");
    }

    [TestMethod]
    public void RequirePermission_WithoutRoles_ShouldRequireAuthorizationWithoutRoles()
    {
        var group = _app.MapGroup("/test");

        var result = group.RequirePermission();

        Check.That(result).IsSameReferenceAs(group);

        var endpoint = GetEndpoint(group, "/dummy-auth1");
        var authorizeData = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>();
        Check.That(authorizeData).IsNotNull();
        Check.That(authorizeData).Not.IsEmpty();

        var hasRolesDefined = authorizeData.Any(a => !string.IsNullOrEmpty(a.Roles));
        Check.That(hasRolesDefined).IsFalse();

        var antiforgeryDisabled = endpoint.Metadata
                                          .Any(m => m.GetType()
                                                     .GetInterfaces()
                                                     .Any(i => i.Name.Contains("Antiforgery")));
        Check.That(antiforgeryDisabled).IsTrue();
    }

    [TestMethod]
    public void RequirePermission_WithRoles_ShouldRequireAuthorizationWithRoles()
    {
        var group = _app.MapGroup("/test");

        var result = group.RequirePermission("Admin,User");

        Check.That(result).IsSameReferenceAs(group);

        var endpoint = GetEndpoint(group, "/dummy-auth2");
        var authorizeData = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>();
        Check.That(authorizeData).IsNotNull();
        Check.That(authorizeData).Not.IsEmpty();
        Check.That(authorizeData.Last().Roles).IsEqualTo("Admin,User");

        var antiforgeryDisabled = endpoint.Metadata
                                          .Any(m => m.GetType()
                                                     .GetInterfaces()
                                                     .Any(i => i.Name.Contains("Antiforgery")));
        Check.That(antiforgeryDisabled).IsTrue();
    }

    private Endpoint GetEndpoint(RouteGroupBuilder group, string path)
    {
        group.MapGet(path, () => Results.Ok());

        var appRouteBuilder = (IEndpointRouteBuilder)_app;
        var endpoints = appRouteBuilder.DataSources
                                       .SelectMany(ds => ds.Endpoints)
                                       .Where(ep => ep.DisplayName?.Contains(path) == true)
                                       .ToList();

        Check.That(endpoints).Not.IsEmpty();
        return endpoints[0];
    }
}