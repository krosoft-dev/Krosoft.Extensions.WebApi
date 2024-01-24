using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.Identity.Abstractions.Fakes;

public class FakeIdentityService : IIdentityService
{
    public string GetId() => throw new NotImplementedException();

    public string GetLangueCode() => throw new NotImplementedException();

    public Guid GetLangueId() => throw new NotImplementedException();

    public string GetNom() => throw new NotImplementedException();

    public string GetProprietaireId() => throw new NotImplementedException();

    public Guid GetRoleId() => throw new NotImplementedException();
    public bool GetRoleIsInterne() => throw new NotImplementedException();

    public string GetTenantId() => throw new NotImplementedException();

    public bool HasDroit(string droitCode) => throw new NotImplementedException();
}