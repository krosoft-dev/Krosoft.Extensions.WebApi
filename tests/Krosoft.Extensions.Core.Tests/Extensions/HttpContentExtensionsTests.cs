using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Krosoft.Extensions.Samples.Library.Models;
using Krosoft.Extensions.Testing;
using Moq.Protected;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Tests.Extensions;

[TestClass]
public class HttpContentExtensionsTests : BaseTest
{
     

    [TestMethod]
    public async Task ReadAsJsonAsync_ParsesJsonContent()
    {
        var item = new Item { Code = "Test" };
        var jsonString = JsonConvert.SerializeObject(item);
        var content = new StringContent(jsonString, Encoding.UTF8, HttpClientExtensions.MediaTypeJson);

        var result = await content.ReadAsJsonAsync<Item>();

        Check.That(result).IsNotNull();
        Check.That(result?.Code).Equals("Test");
    }

    
}