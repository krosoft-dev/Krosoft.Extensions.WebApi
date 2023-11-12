using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly IClaimsService _claimsService;

    public IdentityService(IClaimsService claimsService)
    {
        _claimsService = claimsService;
    }

    public string? GetProprietaireId()
    {
        return _claimsService.CheckClaim(KrosoftClaimNames.ProprietaireId, claim => claim, false);
    }

    public string? GetId()
    {
        return _claimsService.CheckClaim(KrosoftClaimNames.Id, claim => claim, true);
    }

    public Guid GetRoleId()
    {
        return _claimsService.CheckClaim(KrosoftClaimNames.RoleId, claim => new Guid(claim), true);
    }

    public bool GetRoleIsInterne()
    {
        return _claimsService.CheckClaim(KrosoftClaimNames.RoleIsInterne, claim =>
        {
            bool.TryParse(claim, out var r);
            return r;
        }, false);
    }

    public Guid GetLangueId()
    {
        return _claimsService.CheckClaim(KrosoftClaimNames.LangueId, claim => new Guid(claim), true);
    }

    public string? GetLangueCode()
    {
        return _claimsService.CheckClaim(KrosoftClaimNames.LangueCode, claim => claim, true);
    }

    public string? GetNom()
    {
        return _claimsService.CheckClaim(KrosoftClaimNames.Nom, claim => claim, true);
    }

    public bool HasDroit(string droitCode)
    {
        return _claimsService.CheckClaims(KrosoftClaimNames.Droits, droitsCode => droitsCode.Contains(droitCode));
    }

    public string? GetTenantId()
    {
        return _claimsService.CheckClaim(KrosoftClaimNames.TenantId, claim => claim, false);
    }
}