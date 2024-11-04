namespace Krosoft.Extensions.Yarp.Models;

public class CustomReverseProxySettings
{
    public IDictionary<string, ReverseProxyService> Services { get; set; } = new Dictionary<string, ReverseProxyService>();
}