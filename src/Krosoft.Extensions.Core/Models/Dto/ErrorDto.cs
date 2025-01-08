using System.Net;
using Krosoft.Extensions.Core.Attributes;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Core.Models.Dto;

public record ErrorDto
{
    /// <summary>
    /// Code de l'erreur.
    /// </summary>
    public int Code => (int)Status;

    /// <summary>
    /// Message principale de l'erreur.
    /// </summary>
    public string Message => Status.ToString();

    /// <summary>
    /// Liste de toutes les erreurs.
    /// </summary>
    public ISet<string> Errors { get; set; } = new HashSet<string>();

    [JsonIgnore]
    [SwaggerExcludeProperty]
    public HttpStatusCode Status { get; set; }
}