using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Identity.Abstractions.Interfaces;
using Krosoft.Extensions.Identity.Abstractions.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.Identity.Services;

public class TokenProvider : ITokenProvider
{
    private readonly IDataProtectionProvider _dataProtector;
    private readonly IDateTimeService _dateTimeService;
    private readonly TokenSettings _tokenSettings;

    public TokenProvider(IDataProtectionProvider dataProtector, IOptions<TokenSettings> options, IDateTimeService dateTimeService)
    {
        _dataProtector = dataProtector;
        _dateTimeService = dateTimeService;
        _tokenSettings = options.Value;
    }

    public string GenerateToken(string purpose, string securityStamp, string identifier)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(purpose), purpose);
        Guard.IsNotNullOrWhiteSpace(nameof(securityStamp), securityStamp);
        Guard.IsNotNullOrWhiteSpace(nameof(identifier), identifier);

        var ms = new MemoryStream();
        using (var writer = ms.CreateWriter())
        {
            writer.Write(_dateTimeService.Now.Ticks);
            writer.Write(identifier);
            writer.Write(purpose);
            writer.Write(securityStamp);
        }

        var protectedBytes = _dataProtector.CreateProtector(purpose).Protect(ms.ToArray());
        return Convert.ToBase64String(protectedBytes);
    }

    public bool Validate(string purpose, string securityStamp, string identifier, string token)
    {
        Guard.IsNotNullOrWhiteSpace(nameof(purpose), purpose);
        Guard.IsNotNullOrWhiteSpace(nameof(securityStamp), securityStamp);
        Guard.IsNotNullOrWhiteSpace(nameof(identifier), identifier);
        Guard.IsNotNullOrWhiteSpace(nameof(token), token);

        try
        {
            var unprotectedData = _dataProtector.CreateProtector(purpose).Unprotect(Convert.FromBase64String(token));
            var ms = new MemoryStream(unprotectedData);
            using (var reader = ms.CreateReader())
            {
                var creationTime = reader.ReadDateTimeOffset();
                var tokenLifespan = GetTokenLifespan();
                var expirationTime = creationTime + tokenLifespan;
                if (expirationTime < _dateTimeService.Now)
                {
                    return false;
                }

                var identifierFromStream = reader.ReadString();
                if (!string.Equals(identifierFromStream, identifier))
                {
                    return false;
                }

                var purposeFromStream = reader.ReadString();
                if (!string.Equals(purposeFromStream, purpose))
                {
                    return false;
                }

                var securityStampFromStream = reader.ReadString();
                if (reader.PeekChar() != -1)
                {
                    return false;
                }

                return securityStampFromStream == securityStamp;
            }
        }
        catch
        {
            // Do not leak exception
        }

        return false;
    }

    private TimeSpan GetTokenLifespan()
    {
        if (_tokenSettings.TokenLifespan > 0)
        {
            return TimeSpan.FromDays(_tokenSettings.TokenLifespan);
        }

        return TimeSpan.FromDays(1);
    }
}