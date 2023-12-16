using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Samples.Library.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Core.Tests.Tools;

[TestClass]
public class GuardTests
{
    [TestMethod]
    public void IsNotNullKoTest()
    {
        Addresse? addresse = null;

        Check.ThatCode(() => Guard.IsNotNull(nameof(addresse), addresse))
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'addresse' n'est pas renseignée.");
    }

    [TestMethod]
    public void IsNotNullOkTest()
    {
        var item = new Item();

        Check.ThatCode(() => Guard.IsNotNull(nameof(item), item))
             .Not.Throws<KrosoftTechniqueException>();
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpaceKoEmptyTest()
    {
        var pdfFilepath = string.Empty;
        Check.ThatCode(() => Guard.IsNotNullOrWhiteSpace(nameof(pdfFilepath), pdfFilepath))
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'pdfFilepath' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpaceKoWhiteSpaceTest()
    {
        var pdfFilepath = "      ";
        Check.ThatCode(() => Guard.IsNotNullOrWhiteSpace(nameof(pdfFilepath), pdfFilepath))
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'pdfFilepath' est vide ou non renseignée.");
    }

    [TestMethod]
    public void IsNotNullOrWhiteSpaceOkTest()
    {
        var pdfFilepath = "test";
        Check.ThatCode(() => Guard.IsNotNullOrWhiteSpace(nameof(pdfFilepath), pdfFilepath))
             .Not
             .Throws<KrosoftTechniqueException>();
    }
}