namespace Krosoft.Extensions.WebApi.HealthChecks.Models;

public record HealthCheckDto
{
    public string? Status { get; set; }
    public string? Key { get; set; }
    public string? Description { get; set; }
}