using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.WebApi.Extensions;

public static class ControllerBaseExtensions
{
    public static async Task<IEnumerable<string>> GetRequestToBase64StringAsync(this ControllerBase controller)
    {
        var formCollection = await controller.Request.ReadFormAsync();
        var files = await formCollection.ToBase64StringAsync();
        return files;
    }
}