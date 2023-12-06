namespace Krosoft.Extensions.Data.Json.Interfaces;

public interface IJsonDataService<T> where T : class
{
    IEnumerable<T> Query();
    Task InsertAsync(T item, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task UpdateAsync(int id, T item, CancellationToken cancellationToken);
}