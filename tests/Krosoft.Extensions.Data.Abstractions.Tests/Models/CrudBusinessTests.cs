using Krosoft.Extensions.Data.Abstractions.Models;
using Krosoft.Extensions.Samples.Library.Models;
using NFluent;

namespace Krosoft.Extensions.Data.Abstractions.Tests.Models;

[TestClass]
public class CrudBusinessTests
{
    [TestMethod]
    public void Ctor_Ok()
    {
        var toBdd = new CrudBusiness<Item>();

        toBdd.ToDelete.Add(new Item());
        toBdd.ToAdd.Add(new Item());
        toBdd.ToUpdate.Add(new Item());

        Check.That(toBdd.ToDelete).HasSize(1);
        Check.That(toBdd.ToAdd).HasSize(1);
        Check.That(toBdd.ToUpdate).HasSize(1);
    }
}