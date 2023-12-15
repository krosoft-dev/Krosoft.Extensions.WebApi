using System.Reflection;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Pdf.Extensions;
using Krosoft.Extensions.Pdf.Interfaces;
using Krosoft.Extensions.Samples.Library.Factories;
using Krosoft.Extensions.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Pdf.Tests.Services;

[TestClass]
public class PdfServiceTests : BaseTest
{
    private IPdfService _pdfService = null!;

    protected override void AddServices(IServiceCollection services,
                                        IConfiguration configuration) => services.AddPdf();

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _pdfService = serviceProvider.GetRequiredService<IPdfService>();
    }

    [TestMethod]
    public void MergeStreamNull_Ok()
    {
        Check.ThatCode(() => { _pdfService.Merge((Stream[])null!); })
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'streams' n'est pas renseignée.");
    }

    [TestMethod]
    public void MergeByteNull_Ok()
    {
        Check.ThatCode(() => { _pdfService.Merge((byte[][])null!); })
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'files' n'est pas renseignée.");
    }

    [TestMethod]
    public void MergeStreamEmpty_Ok()
    {
        var stream = _pdfService.Merge(new List<Stream>().ToArray());

        Check.That(stream).IsNotNull();
    }

    [TestMethod]
    public void MergeByteEmpty_Ok()
    {
        var stream = _pdfService.Merge(new List<byte[]>().ToArray());

        Check.That(stream).IsNotNull();
    }

    [TestMethod]
    public void MergeStreams_Ok()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var pdf1 = AssemblyHelper.Read(assembly, "sample1.pdf");
        Check.That(pdf1).IsNotNull();
        Check.That(pdf1.Length).IsEqualTo(13264);

        var pdf2 = AssemblyHelper.Read(assembly, "sample1.pdf");
        Check.That(pdf2).IsNotNull();
        Check.That(pdf2.Length).IsEqualTo(13264);

        var data = _pdfService.Merge(pdf1,
                                     pdf2);
        FileHelper.CreateFile("sample-stream.pdf", data);

        Check.That(data).IsNotNull();
    }

    [TestMethod]
    public void MergeBytes_Ok()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var pdf1 = AssemblyHelper.Read(assembly, "sample1.pdf").ToByte();
        var pdf2 = AssemblyHelper.Read(assembly, "sample1.pdf").ToByte();

        var data = _pdfService.Merge(pdf1,
                                     pdf2);
        FileHelper.CreateFile("sample-byte.pdf", data);

        Check.That(data).IsNotNull();
    }

    [TestMethod]
    public void PdfFileStream_Ok()
    {
        var assembly = typeof(AddresseFactory).Assembly;

        var file = AssemblyHelper.ReadAsString(assembly, "sample1.pdf", EncodingHelper.GetEuropeOccidentale());
        Check.That(file).IsNotNull();
        Check.That(file.Length).IsEqualTo(13264);
        Check.That(file).StartsWith("%PDF-1.4");

        var pdf1 = AssemblyHelper.Read(assembly, "sample1.pdf");
        Check.That(pdf1).IsNotNull();
        Check.That(pdf1.Length).IsEqualTo(13264);

        var pdf2 = AssemblyHelper.Read(assembly, "sample1.pdf");
        Check.That(pdf2).IsNotNull();
        Check.That(pdf2.Length).IsEqualTo(13264);

        var data = _pdfService.Merge(pdf1, pdf2);

        var pdffile = new PdfFileStream(data, "Logiciels.pdf");
        Check.That(pdffile).IsNotNull();
        Check.That(pdffile.FileName).IsEqualTo("Logiciels.pdf");
        Check.That(pdffile.ContentType).IsEqualTo("application/pdf");
        Check.That(pdffile.Stream).IsNotNull();
        Check.That(pdffile.Stream.CanRead).IsTrue();
    }
}