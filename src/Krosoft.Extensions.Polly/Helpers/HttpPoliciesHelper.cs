using Krosoft.Extensions.Polly.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;

namespace Krosoft.Extensions.Polly.Helpers;

public static class HttpPoliciesHelper
{
    public static PolicyBuilder<HttpResponseMessage> GetBaseBuilder() => HttpPolicyExtensions.HandleTransientHttpError();

    public static IAsyncPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy(ILogger logger, ICircuitBreakerPolicyConfig circuitBreakerPolicyConfig)
    {
        return GetBaseBuilder()
            .CircuitBreakerAsync(circuitBreakerPolicyConfig.RetryCount,
                                 TimeSpan.FromSeconds(circuitBreakerPolicyConfig.BreakDuration),
                                 (result, breakDuration) =>

                                 {
                                     if (result == null)
                                     {
                                         logger.LogWarning($"Service shutdown during {breakDuration} after {circuitBreakerPolicyConfig.RetryCount} failed retries.");

                                         throw new BrokenCircuitException("Service inoperative. Please try again later...");
                                     }

                                     if (result.Exception != null)
                                     {
                                         logger.LogWarning($"Service shutdown during {breakDuration} after {circuitBreakerPolicyConfig.RetryCount} failed retries : {result.Result?.StatusCode} {result.Exception.Message}");

                                         throw new BrokenCircuitException($"Service inoperative. Please try again later : {result.Result?.StatusCode} {result.Exception.Message}", result.Exception);
                                     }

                                     if (result.Result != null)
                                     {
                                         var message = result.Result.Content.ReadAsStringAsync().Result;

                                         logger.LogWarning($"Service shutdown during {breakDuration} after {circuitBreakerPolicyConfig.RetryCount} failed retries : {result.Result.StatusCode} {message}");

                                         throw new BrokenCircuitException($"Service inoperative. Please try again later : {result.Result.StatusCode} {message}");
                                     }
                                 },
                                 () => { logger.LogInformation("Service restarted."); });
    }

    public static IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy(ILogger logger, IRetryPolicyConfig retryPolicyConfig)
    {
        return GetBaseBuilder()
            .WaitAndRetryAsync(retryPolicyConfig.RetryCount,
                               retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryPolicyConfig.BackoffPower, retryAttempt)) + TimeSpan.FromMilliseconds(new Random().Next(0, 100)),
                               (result, timeSpan, retryCount, _) =>
                               {
                                   if (result.Result != null)
                                   {
                                       logger.LogWarning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                                   }
                                   else
                                   {
                                       logger.LogWarning($"Request failed because network failure. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                                   }
                               });
    }
}