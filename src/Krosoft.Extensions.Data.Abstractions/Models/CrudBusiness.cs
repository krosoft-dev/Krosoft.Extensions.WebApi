namespace Krosoft.Extensions.Data.Abstractions.Models;

public record CrudBusiness<T>
{
    public ICollection<T> ToUpdate { get; } = new List<T>();
    public ICollection<T> ToAdd { get; } = new List<T>();
    public ICollection<T> ToDelete { get; } = new List<T>();
}