using System.Security.Claims;
using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IClaimsBuilderService
{
    IEnumerable<Claim> Build(KrosoftToken? krosoftToken);
}