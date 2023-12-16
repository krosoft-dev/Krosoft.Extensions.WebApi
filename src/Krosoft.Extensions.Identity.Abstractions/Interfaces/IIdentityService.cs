namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IIdentityService
{
    string? GetId();
    string? GetLangueCode();
    Guid GetLangueId();
    string? GetNom();
    string? GetProprietaireId();
    Guid GetRoleId();
    bool GetRoleIsInterne();
    string? GetTenantId();
    bool HasDroit(string droitCode);
}