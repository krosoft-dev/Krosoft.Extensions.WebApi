using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Samples.DotNet8.BlazorApp.Models;

namespace Krosoft.Extensions.Samples.DotNet8.BlazorApp.Interfaces;

public interface ILogicielsHttpService
{
    Task<Result<IEnumerable<Logiciel>?>> GetLogicielsAsync(string text,
                                                           CancellationToken cancellationToken);
}