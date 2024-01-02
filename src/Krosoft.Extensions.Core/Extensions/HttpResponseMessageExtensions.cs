using System.Net;
using Krosoft.Extensions.Core.Converters;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Models.Exceptions.Http;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<T?> EnsureAsync<T>(this HttpResponseMessage httpResponseMessage,
                                                CancellationToken cancellationToken = default)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        await httpResponseMessage.EnsureAsync(cancellationToken);
        return default;
    }

    public static async Task EnsureAsync(this HttpResponseMessage httpResponseMessage,
                                         CancellationToken cancellationToken = default)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
        }
        else
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

            var isValid = JsonHelper.IsValid(json);
            if (isValid && httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
            {
                var obj = JsonConvert.DeserializeObject<KrosoftMetierException>(json, new KrosoftMetierExceptionConverter());
                if (obj != null)
                {
                    throw obj;
                }
            }

            throw new HttpException(httpResponseMessage.StatusCode,
                                    httpResponseMessage.ReasonPhrase);
        }
    }

    public static async Task ManageErrorAsync(this HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
    {
        var json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

        var isValid = JsonHelper.IsValid(json);
        if (isValid)
        {
            var obj = JsonConvert.DeserializeObject<ErrorApi>(json);
            if (obj != null)
            {
                if (Enum.TryParse(obj.StatusCode, out HttpStatusCode value) && Enum.IsDefined(typeof(HttpStatusCode), value))
                {
                    throw new HttpException(value, obj.Message);
                }

                throw new HttpException(httpResponseMessage.StatusCode,
                                        obj.Message);
            }
        }

        throw new HttpException(httpResponseMessage.StatusCode,
                                httpResponseMessage.ReasonPhrase);
    }
}