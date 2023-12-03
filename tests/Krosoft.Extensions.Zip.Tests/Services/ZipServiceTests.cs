using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.Zip.Extensions;
using Krosoft.Extensions.Zip.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Krosoft.Extensions.Zip.Tests.Services;

[TestClass]
public class ZipServiceTests : BaseTest
{
    private IZipService _zipService = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddZip();
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _zipService = serviceProvider.GetRequiredService<IZipService>();
    }

    [TestMethod]
    public async Task ZipAsync_Ok()
    {
        var filesPath = new List<string>();
        filesPath.Add("Files/fichier1.txt");
        filesPath.Add("Files/fichier2.txt");
        filesPath.Add("Files/fichier3.txt");

        var dictionary = new Dictionary<string, string>();
        foreach (var filePath in filesPath)
        {
            dictionary.Add(Path.GetFileName(filePath), filePath);
        }

        var date = DateTime.Now;
        var zipPath = $"UnitTest_{date.Year}_{date.Month}_{date.Day}_{date.Hour}_{date.Minute}_{date.Second}.zip";

        var zipfile = await _zipService.ZipAsync(dictionary, zipPath, CancellationToken.None);

        Check.That(File.Exists(zipPath)).IsFalse();
        Check.That(zipfile.ContentType).IsEqualTo("application/zip");
        Check.That(zipfile.FileName).IsEqualTo(zipPath);
        Check.That(zipfile.Stream).IsNotNull();
        Check.That(zipfile.Stream.CanRead).IsTrue();

        await FileHelper.WriteAsync(zipPath, zipfile.Stream, CancellationToken.None);
        Check.That(File.Exists(zipPath)).IsTrue();
    }

    [TestMethod]
    public void ExtractZip_Ok()
    {
        var extractPath = "ExtractZipTest";

        _zipService.ExtractZip("Files/zip.zip", extractPath);

        var numberFileExtract = Directory.GetFiles(extractPath).Select(Path.GetFileName).ToList();
        Check.That(numberFileExtract.Count).IsEqualTo(3);
        Check.That(numberFileExtract).Contains("file1.txt", "file2.txt", "file3.txt");
    }

    [TestMethod]
    public void ZipStreams_Ok()
    {
        var streams = new Dictionary<string, Stream>();
        var date = DateTime.Now;
        var filePath = $"UnitTest_{date.Year}_{date.Month}_{date.Day}_{date.Hour}_{date.Minute}_{date.Second}.zip";
        var extractPath = "UnitTestExtract";

        streams.Add("file1.txt", File.OpenRead("Files/fichier1.txt"));
        streams.Add("file2.txt", File.OpenRead("Files/fichier2.txt"));
        streams.Add("file3.txt", File.OpenRead("Files/fichier3.txt"));

        var zip = _zipService.Zip(streams);
        FileHelper.Write(filePath, zip);

        Check.That(File.Exists(filePath)).IsTrue();

        _zipService.ExtractZip(filePath, extractPath);

        var numberFileExtract = Directory.GetFiles(extractPath).Select(Path.GetFileName).ToList();
        Check.That(numberFileExtract.Count).IsEqualTo(3);
        Check.That(numberFileExtract).Contains("file1.txt", "file2.txt", "file3.txt");
    }

    [TestMethod]
    public void Zip_Null()
    {
        Check.ThatCode(() => _zipService.Zip(null!))
             .Throws<KrosoftTechniqueException>()
             .WithMessage("La variable 'streams' n'est pas renseignée.");
    }
}