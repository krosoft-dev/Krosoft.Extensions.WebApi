using System.Security.Claims;
using Krosoft.Extensions.Core.Models;
using Newtonsoft.Json;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Identity.Abstractions.Constantes;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;

namespace Krosoft.Extensions.Identity.Services;

public class ClaimsBuilderService : IClaimsBuilderService
{
    public IEnumerable<Claim> Build(KrosoftToken? krosoftToken)
    {
        Guard.IsNotNull(nameof(krosoftToken), krosoftToken);
        Guard.IsNotNullOrWhiteSpace(nameof(krosoftToken.Id), krosoftToken!.Id);
        Guard.IsNotNullOrWhiteSpace(nameof(krosoftToken.LangueId), krosoftToken.LangueId);
        Guard.IsNotNullOrWhiteSpace(nameof(krosoftToken.RoleId), krosoftToken.RoleId);

        var claims = new List<Claim>();
        if (krosoftToken.Id != null)
        {
            claims.Add(new Claim(KrosoftClaimNames.Id, krosoftToken.Id, ClaimValueTypes.String));
        }

        if (krosoftToken.Nom != null)
        {
            claims.Add(new Claim(KrosoftClaimNames.Nom, krosoftToken.Nom, ClaimValueTypes.String));
        }

        if (krosoftToken.Email != null)
        {
            claims.Add(new Claim(KrosoftClaimNames.Email, krosoftToken.Email, ClaimValueTypes.Email));
        }

        if (krosoftToken.RoleId != null)
        {
            claims.Add(new Claim(KrosoftClaimNames.RoleId, krosoftToken.RoleId, ClaimValueTypes.String));
        }

        claims.Add(new Claim(KrosoftClaimNames.RoleIsInterne, krosoftToken.RoleIsInterne.ToString(), ClaimValueTypes.Boolean));
        if (krosoftToken.RoleHomePage != null)
        {
            claims.Add(new Claim(KrosoftClaimNames.RoleHomePage, krosoftToken.RoleHomePage, ClaimValueTypes.String));
        }

        if (krosoftToken.LangueId != null)
        {
            claims.Add(new Claim(KrosoftClaimNames.LangueId, krosoftToken.LangueId, ClaimValueTypes.String));
        }

        if (krosoftToken.LangueCode != null)
        {
            claims.Add(new Claim(KrosoftClaimNames.LangueCode, krosoftToken.LangueCode, ClaimValueTypes.String));
        }

        claims.Add(new Claim(KrosoftClaimNames.Droits, JsonConvert.SerializeObject(krosoftToken.DroitsCode),
                             CustomClaimTypes.JsonArray));

        if (!string.IsNullOrEmpty(krosoftToken.TenantId))
        {
            claims.Add(new Claim(KrosoftClaimNames.TenantId, krosoftToken.TenantId, ClaimValueTypes.String));
        }

        if (!string.IsNullOrEmpty(krosoftToken.ProprietaireId))
        {
            claims.Add(new Claim(KrosoftClaimNames.ProprietaireId, krosoftToken.ProprietaireId, ClaimValueTypes.String));
        }

        return claims;
    }
}