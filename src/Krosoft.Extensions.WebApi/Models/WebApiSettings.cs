namespace Krosoft.Extensions.WebApi.Models;

public record WebApiSettings
{
    public string[] AllowedOrigins { get; set; } = [];
    public string[] ExposedHeaders { get; set; } = [];
    public string[] Cultures { get; set; } = [];
}