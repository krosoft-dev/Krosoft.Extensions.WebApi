using System.Net;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichier;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Functional;

[TestClass]
public class DocumentsEndpointTests : SampleBaseApiTest<Program>
{
    [TestMethod]
    public async Task Deposer_Ok()
    {
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent("42"), "FichierId");
        form.Add(new ByteArrayContent(ByteHelper.GetBytes("Hello")), "File", "test.txt");

        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsync("/Documents/Deposer/Fichier", form);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var depotDto = await response.Content.ReadAsNewtonsoftJsonAsync<DepotDto>(CancellationToken.None);
        Check.That(depotDto).IsNotNull();

        Check.That(depotDto!.Message).StartsWith("Fichier dispo sur temp");
    }

    [TestMethod]
    public async Task DeposerSansRetour_Ok()
    {
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent("42"), "FichierId");
        form.Add(new ByteArrayContent(ByteHelper.GetBytes("Hello")), "File", "test.txt");

        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsync("/Documents/Deposer/Fichier/SansRetour", form);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        Check.That(result).IsEmpty();
    }

    [TestMethod]
    public async Task Deposer_Error()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsync("/Documents/Deposer/Fichier", null);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.InternalServerError);
        var errorDto = await response.Content.ReadAsNewtonsoftJsonAsync<ErrorDto>(CancellationToken.None);
        Check.That(errorDto).IsNotNull();
        Check.That(errorDto!.Message).StartsWith("InternalServerError");
        Check.That(errorDto.Errors).ContainsExactly("Unexpected request without body, failed to bind parameter \"long FichierId\" from the request body as form.");
    }

    [TestMethod]
    public async Task Deposer_BadRequest()
    {
        using var form = new MultipartFormDataContent();
        form.Add(new StringContent("0"), "FichierId");
        form.Add(new ByteArrayContent(ByteHelper.GetBytes("Hello")), "File", "test.txt");
        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsync("/Documents/Deposer/Fichier", form);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        var errorDto = await response.Content.ReadAsNewtonsoftJsonAsync<ErrorDto>(CancellationToken.None);
        Check.That(errorDto).IsNotNull();
        Check.That(errorDto!.Message).StartsWith("BadRequest");
        Check.That(errorDto.Errors).ContainsExactly("'Fichier Id' ne doit pas être vide.");
    }
}