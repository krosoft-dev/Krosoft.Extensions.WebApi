using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Services;
using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Data;

public class SampleSeedService<T> : SeedService<T> where T : KrosoftContext
{
    protected override void BeforeSave(T db)
    {
        db.Import<Logiciel>();
        db.Import<Langue>();
        db.Import<Pays>();
    }
}