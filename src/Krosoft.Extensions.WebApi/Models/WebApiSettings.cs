namespace Krosoft.Extensions.WebApi.Models;

public class WebApiSettings
{
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
    public string[] ExposedHeaders { get; set; } = Array.Empty<string>();
}