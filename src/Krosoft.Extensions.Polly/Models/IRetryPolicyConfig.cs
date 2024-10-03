namespace Krosoft.Extensions.Polly.Models;

public interface IRetryPolicyConfig
{
    int RetryCount { get; set; }
    int BackoffPower { get; set; }
}