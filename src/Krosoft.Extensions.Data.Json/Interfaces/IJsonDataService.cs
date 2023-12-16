namespace Krosoft.Extensions.Data.Json.Interfaces;

public interface IJsonDataService<T> where T : class
{
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task InsertAsync(T item, CancellationToken cancellationToken);
    IEnumerable<T> Query();
    Task UpdateAsync(int id, T item, CancellationToken cancellationToken);
}