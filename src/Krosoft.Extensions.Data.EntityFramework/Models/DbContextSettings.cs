using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public class DbContextSettings<T> : IDbContextSettings<T> where T : KrosoftContext;