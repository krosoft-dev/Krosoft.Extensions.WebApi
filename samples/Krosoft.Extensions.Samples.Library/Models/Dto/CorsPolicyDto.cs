namespace Krosoft.Extensions.Samples.Library.Models.Dto;

public class CorsPolicyDto
{

    public ICollection<string> Origins { get; set; } = new List<string>();
}