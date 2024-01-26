using System.Net.Http.Headers;
using System.Text;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Samples.DotNet8.BlazorApp.Interfaces;
using Krosoft.Extensions.Samples.DotNet8.BlazorApp.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Krosoft.Extensions.Samples.DotNet8.BlazorApp.Services;

public class LogicielsHttpService : ILogicielsHttpService
{
    private readonly HttpClient _httpClient;

    public LogicielsHttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<Logiciel>?>> GetLogicielsAsync(string text,
                                                                        CancellationToken cancellationToken)
    {
        try
        {
            var uri = Urls.Api.GetLogiciels(text);
            var responseMessage = await _httpClient.GetAsync(uri, cancellationToken);
            if (responseMessage.IsSuccessStatusCode)
            {
                await using var responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
                var logiciels = await JsonSerializer.DeserializeAsync<IEnumerable<Logiciel>>(responseStream, cancellationToken: cancellationToken);
                return new Result<IEnumerable<Logiciel>?>(logiciels);
            }

            var responseJson = await responseMessage.Content.ReadAsStringAsync();

            return new Result<IEnumerable<Logiciel>?>(new Exception(responseMessage.StatusCode.ToString()));
        }
        catch (Exception e)
        {
            return new Result<IEnumerable<Logiciel>?>(e);
        }
    }

    public async Task<Result<Guid>> CreateAsync(Logiciel logiciel,
                                                CancellationToken cancellationToken)
    {
        try
        {
            var uri = Urls.Api.CreateLogiciel(); 

            var responseMessage = await _httpClient.PostAsJsonAsync(uri, logiciel, cancellationToken);
            if (responseMessage.IsSuccessStatusCode)
            {
                var s = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                return new Result<Guid>(new Guid(s));
            }

            var responseJson = await responseMessage.Content.ReadAsStringAsync();

            return new Result<Guid>(new Exception(responseMessage.StatusCode.ToString()));
        }
        catch (Exception e)
        {
            return new Result<Guid>(e);
        }
    }
}