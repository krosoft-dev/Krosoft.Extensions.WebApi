using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.Identity.Services;

public class KrosoftTokenBuilderService : IKrosoftTokenBuilderService
{
    private readonly IIdentityService _identityService;

    public KrosoftTokenBuilderService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public KrosoftToken Build()
    {
        var krosoftToken = new KrosoftToken
        {
            Id = _identityService.GetId(),
            TenantId = _identityService.GetTenantId(),
            Nom = _identityService.GetNom(),
            Email = null,
            ProprietaireId = _identityService.GetProprietaireId(),
            RoleId = _identityService.GetRoleId().ToString(),
            RoleIsInterne = _identityService.GetRoleIsInterne(),
            RoleHomePage = null,
            LangueId = _identityService.GetLangueId().ToString(),
            LangueCode = _identityService.GetLangueCode()
        };

        return krosoftToken;
    }
}