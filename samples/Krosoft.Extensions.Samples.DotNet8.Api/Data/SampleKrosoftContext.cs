using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Data;

public class SampleKrosoftContext : KrosoftContext
{
    public SampleKrosoftContext(DbContextOptions options)
        : base(options)
    {
    }
}