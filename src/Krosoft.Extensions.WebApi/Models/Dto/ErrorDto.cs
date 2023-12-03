using System.Net;
using Newtonsoft.Json;

namespace Krosoft.Extensions.WebApi.Models.Dto;

public class ErrorDto
{
    public ErrorDto()
    {
        Errors = new HashSet<string>();
    }

    [JsonIgnore]
    public HttpStatusCode Status { get; set; }

    public int Code => (int)Status;
    public string Message => Status.ToString();
    public ISet<string> Errors { get; set; }
}