namespace Krosoft.Extensions.WebApi.HealthChecks.Models;

public class HealthCheckStatusModel
{
    public string? Status { get; set; }
    public IEnumerable<HealthCheckModel> Checks { get; set; } = new List<HealthCheckModel>();
    public string? Duration { get; set; }
    public string? Environnement { get; set; }
}