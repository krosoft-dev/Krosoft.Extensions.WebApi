using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet6.Api.Models.Commands;

public abstract class LogicielBaseCommand<TReturn> : AuthBaseCommand<TReturn>
{
    public string? Nom { get; set; }
    public Guid CategorieId { get; set; }
}