namespace Krosoft.Extensions.WebApi.Identity.Models;

public record WebApiIdentySettings
{
    public IEnumerable<KeySettings> Keys { get; set; } = new List<KeySettings>();
}