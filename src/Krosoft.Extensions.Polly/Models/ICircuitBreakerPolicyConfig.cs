namespace Krosoft.Extensions.Polly.Models;

public interface ICircuitBreakerPolicyConfig
{
    int RetryCount { get; set; }
    int BreakDuration { get; set; }
}