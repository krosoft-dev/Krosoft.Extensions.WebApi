namespace Krosoft.Extensions.Identity.Abstractions.Models;

public record HashSalt
{
    public byte[]? Hash { get; set; }
    public byte[]? Salt { get; set; }
}