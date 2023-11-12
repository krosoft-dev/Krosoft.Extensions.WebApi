using System.Security.Cryptography;
using System.Text;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;

namespace Krosoft.Extensions.Identity.Services;

public class SimplePasswordHasher : ISimplePasswordHasher
{
    public bool Verify(string password,
                       byte[]? hash,
                       byte[]? salt)
    {
        Guard.IsNotNull(nameof(password), password);
        Guard.IsNotNull(nameof(hash), hash);
        Guard.IsNotNull(nameof(salt), salt);

        using (var hmac = new HMACSHA512(salt!))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return hash!.SequenceEqual(computedHash);
        }
    }

    public HashSalt CreatePasswordHash(string password)
    {
        Guard.IsNotNull(nameof(password), password);
        var hs = new HashSalt();
        using (var hmac = new HMACSHA512())
        {
            hs.Salt = hmac.Key;
            hs.Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        return hs;
    }
}