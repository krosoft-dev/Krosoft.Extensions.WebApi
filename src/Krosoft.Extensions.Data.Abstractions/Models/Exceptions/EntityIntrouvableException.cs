using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Data.Abstractions.Models.Exceptions;

public class EntityIntrouvableException<T> : KrosoftTechnicalException where T : Entity
{
    public EntityIntrouvableException(string id) : base($"{typeof(T).Name} {id} introuvable.")
    {
    }

    public EntityIntrouvableException(Guid id) : this(id.ToString())
    {
    }

    public EntityIntrouvableException(IEnumerable<Guid> ids) : base($"{typeof(T).Name} {string.Join(",", ids)} introuvables.")
    {
    }
}