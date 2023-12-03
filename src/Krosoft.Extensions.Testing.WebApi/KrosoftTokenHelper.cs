using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Testing.WebApi;

public static class KrosoftTokenHelper
{
    public static KrosoftToken Defaut => new KrosoftToken
    {
        Id = "Claim_Id",
        TenantId = "Claim_TenantId",
        Nom = "Claim_Nom",
        Email = "Claim_Email",
        RoleId = "Claim_RoleId",
        RoleHomePage = "Claim_RoleHomePage",
        LangueId = "Claim_LangueId",
        LangueCode = "Claim_LangueCode"
    };
}