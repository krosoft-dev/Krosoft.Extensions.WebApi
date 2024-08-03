using System.Net;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Samples.DotNet8.Api.Tests.Core;
using Krosoft.Extensions.Samples.Library.Models.Dto;

namespace Krosoft.Extensions.Samples.DotNet8.Api.Tests.Functional;

[TestClass]
public class LogicielsControllerTests : SampleBaseApiTest<Startup>
{
    private static async Task CheckExportFile(HttpResponseMessage response, string fileNameExpected)
    {
        var content = await response.Content.ReadAsStringAsync(CancellationToken.None);
        Console.WriteLine(content);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

        var fileName = response.Content.Headers.ContentDisposition?.FileName;

        Check.That(fileName).IsEqualTo(fileNameExpected);

        var stream = await response.Content.ReadAsStreamAsync(CancellationToken.None);
        Check.That(stream).IsNotNull();
        Check.That(stream.CanRead).IsTrue();

        await FileHelper.WriteAsync(fileName!, stream, CancellationToken.None);
        Check.That(File.Exists(fileName)).IsTrue();
    }

    [TestMethod]
    public async Task Csv_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync("/Logiciels/Export/Csv");

        await CheckExportFile(response, "Logiciels.csv");
    }

    [TestMethod]
    public async Task Logiciels_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync("/Logiciels");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var logiciels = await response.Content.ReadAsJsonAsync<IEnumerable<LogicielDto>>(CancellationToken.None).ToList();
        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(10);
    }

    [TestMethod]
    public async Task Logiciels_Query_Ok()
    {
        const string nom = "Excel";

        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync($"/Logiciels?Nom={nom}");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var logiciels = await response.Content.ReadAsJsonAsync<IEnumerable<LogicielDto>>(CancellationToken.None).ToList();
        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(10);
    }

    [TestMethod]
    public async Task Pdf_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync("/Logiciels/Export/Pdf");

        await CheckExportFile(response, "Logiciels.pdf");
    }

    [TestMethod]
    public async Task Zip_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync("/Logiciels/Export/Zip");

        await CheckExportFile(response, "Logiciels.zip");
    }
}