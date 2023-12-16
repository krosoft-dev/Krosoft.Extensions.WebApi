namespace Krosoft.Extensions.Identity.Abstractions.Models;

public class HashSalt
{
    public byte[]? Hash { get; set; }
    public byte[]? Salt { get; set; }
}