namespace Krosoft.Extensions.WebApi.HealthChecks.Models;

public record HealthCheckStatusDto
{
    public string? Status { get; set; }
    public string? Duration { get; set; }
    public string? Environnement { get; set; }
    public IEnumerable<HealthCheckDto> Checks { get; set; } = new List<HealthCheckDto>();
}