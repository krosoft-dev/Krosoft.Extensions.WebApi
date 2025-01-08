using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.Library.Models.Commands;

public abstract record LogicielBaseCommand<TReturn> : AuthBaseCommand<TReturn>
{
    public string? Nom { get; set; }
    public Guid CategorieId { get; set; }
}