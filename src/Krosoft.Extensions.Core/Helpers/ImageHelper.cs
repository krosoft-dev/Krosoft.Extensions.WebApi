using System.Text.RegularExpressions;
using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Core.Helpers;

public static class ImageHelper
{
    public static void VerifierImage(string base64, string nomImage)
    {
        var match = Regex.Match(base64, @"data:(?<type>.+?);base64,(?<data>.+)");
        var base64Data = match.Groups["data"].Value;
        if (string.IsNullOrEmpty(base64Data))
        {
            throw new KrosoftTechniqueException($"Impossible de récupérer l'image à partir de l'url Base64 pour l'image {nomImage}.");
        }
    }
}