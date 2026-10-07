using System.Net;
using JetBrains.Annotations;
using Krosoft.Extensions.WebApi.Identity.Models;
using Krosoft.Extensions.WebApi.Identity.Services;
using Krosoft.Extensions.WebApi.Identity.Tests.Fakes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.WebApi.Identity.Tests.Services;

[TestClass]
[TestSubject(typeof(PermissionAuthorizationResultHandler))]
public class PermissionAuthorizationResultHandlerTests
{
    private static readonly AuthorizationPolicy Policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    private static DefaultHttpContext CreateContext()
    {
        var context = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection().AddSingleton<IAuthenticationService, FakeAuthenticationService>().BuildServiceProvider()
        };
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<string> ReadBodyAsync(HttpContext context)
    {
        context.Response.Body.Position = 0;
        return await new StreamReader(context.Response.Body).ReadToEndAsync();
    }

    private static PermissionAuthorizationResultHandler CreateHandler(PermissionErrorOptions? options = null)
        => new(Options.Create(options ?? new PermissionErrorOptions()));

    [TestMethod]
    public async Task Forbidden_IndiqueLaPermissionManquante()
    {
        var context = CreateContext();
        var failure = AuthorizationFailure.Failed([new RolesAuthorizationRequirement(["crm.leads.create"])]);

        await CreateHandler().HandleAsync(_ => Task.CompletedTask, context, Policy, PolicyAuthorizationResult.Forbid(failure));

        Check.That(context.Response.StatusCode).IsEqualTo((int)HttpStatusCode.Forbidden);
        Check.That(await ReadBodyAsync(context)).Contains("Permission requise : crm.leads.create.");
        Check.That(context.Items[PermissionAuthorizationResultHandler.ErrorItemKey]).IsEqualTo("Permission requise : crm.leads.create.");
    }

    [TestMethod]
    public async Task Forbidden_SansExigenceDeRole_MessageParDefaut()
    {
        var context = CreateContext();

        await CreateHandler(new PermissionErrorOptions { ForbiddenMessage = "Interdit." })
            .HandleAsync(_ => Task.CompletedTask, context, Policy, PolicyAuthorizationResult.Forbid());

        Check.That(context.Response.StatusCode).IsEqualTo((int)HttpStatusCode.Forbidden);
        Check.That(await ReadBodyAsync(context)).Contains("Interdit.");
    }

    [TestMethod]
    public async Task Challenged_UtiliseLeMessageConfigureEtConserveLeChallenge()
    {
        var context = CreateContext();
        var options = new PermissionErrorOptions { UnauthorizedMessage = _ => "En-tête x-api-key requis." };

        await CreateHandler(options).HandleAsync(_ => Task.CompletedTask, context, Policy, PolicyAuthorizationResult.Challenge());

        Check.That(context.Response.StatusCode).IsEqualTo((int)HttpStatusCode.Unauthorized);
        Check.That(context.Response.Headers.WWWAuthenticate.ToString()).IsEqualTo("Bearer");
        Check.That(await ReadBodyAsync(context)).Contains("En-tête x-api-key requis.");
    }

    [TestMethod]
    public async Task Succeeded_AppelleLEndpointSansCorpsDErreur()
    {
        var context = CreateContext();
        var called = false;

        await CreateHandler().HandleAsync(_ =>
                                          {
                                              called = true;
                                              return Task.CompletedTask;
                                          },
                                          context,
                                          Policy,
                                          PolicyAuthorizationResult.Success());

        Check.That(called).IsTrue();
        Check.That(await ReadBodyAsync(context)).IsEmpty();
        Check.That(context.Items.ContainsKey(PermissionAuthorizationResultHandler.ErrorItemKey)).IsFalse();
    }
}
