using System.Net;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichier;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Functional;

[TestClass]
public class DocumentsEndpointTests : SampleBaseApiTest<Program>
{
    [TestMethod]
    public async Task Get_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsync("/Documents/Deposer/Fichier", null);

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var depotDto = await response.Content.ReadAsNewtonsoftJsonAsync<DepotDto>(CancellationToken.None);
        Check.That(depotDto).IsNotNull();
    }
}