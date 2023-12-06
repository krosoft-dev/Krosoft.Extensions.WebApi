namespace Krosoft.Extensions.WebApi.HealthChecks.Models;

public static class Urls
{
    public static class Health
    {
        public const string Check = $"/{nameof(Health)}/Check";
        public const string Readiness = $"/{nameof(Health)}/Readiness";
        public const string Liveness = $"/{nameof(Health)}/Liveness";
    }
}