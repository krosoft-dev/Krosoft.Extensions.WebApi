using Krosoft.Extensions.Identity.Abstractions.Models;

namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface ISimplePasswordHasher
{
    HashSalt CreatePasswordHash(string password);

    bool Verify(string password,
                byte[]? hash,
                byte[]? salt);
}