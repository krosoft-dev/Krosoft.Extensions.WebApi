﻿using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.WebApi.Identity.Models.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Krosoft.Extensions.WebApi.Identity.Middlewares;

public class MissingTenantMiddleware
{
    private readonly IIdentityService _identityService;
    private readonly RequestDelegate _next;

    public MissingTenantMiddleware(RequestDelegate next, IIdentityService identityService)
    {
        _next = next;
        _identityService = identityService;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
        {
            //Si on est connecté, on cherck le tenant.
            var tenantId = _identityService.GetUniqueTenantId();
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new TenantNotFoundException();
            }
        }

        await _next.Invoke(httpContext);
    }
}

