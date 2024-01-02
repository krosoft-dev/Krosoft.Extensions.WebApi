using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public interface IDbContextSettings<T> where T : KrosoftContext;