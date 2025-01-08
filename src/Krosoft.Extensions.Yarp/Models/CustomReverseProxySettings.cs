namespace Krosoft.Extensions.Yarp.Models;

public record CustomReverseProxySettings
{
    public IDictionary<string, ReverseProxyService> Services { get; set; } = new Dictionary<string, ReverseProxyService>();
}