using Krosoft.Extensions.Identity.Abstractions.Interfaces;

namespace Krosoft.Extensions.Identity.Abstractions.Fakes;

public class FakeIdentifierProvider : IIdentifierProvider
{
    public Task<string?> GetIdentifierAsync(CancellationToken cancellationToken) => Task.FromResult("Hello")!;
}