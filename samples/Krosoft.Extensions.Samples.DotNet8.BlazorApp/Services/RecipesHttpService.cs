using System.Text.Json;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Samples.DotNet8.BlazorApp.Interfaces;
using Krosoft.Extensions.Samples.DotNet8.BlazorApp.Models;

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

            return new Result<IEnumerable<Logiciel>?>(new Exception(responseMessage.StatusCode.ToString()));
        }
        catch (Exception e)
        {
            return new Result<IEnumerable<Logiciel>?>(e);
        }
    }
}