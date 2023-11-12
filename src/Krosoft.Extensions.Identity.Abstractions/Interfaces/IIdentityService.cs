namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IIdentityService
{
    string? GetId();
    string? GetProprietaireId();
    string? GetNom();
    string? GetTenantId();
    Guid GetRoleId();
    bool GetRoleIsInterne();
    Guid GetLangueId();
    string? GetLangueCode();
    bool HasDroit(string droitCode);
}