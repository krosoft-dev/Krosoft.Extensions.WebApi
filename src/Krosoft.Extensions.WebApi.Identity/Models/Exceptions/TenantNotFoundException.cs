using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.WebApi.Identity.Models.Exceptions;

public class TenantNotFoundException : KrosoftTechnicalException
{
    public TenantNotFoundException() : base("Tenant introuvable.")
    {
    }
}