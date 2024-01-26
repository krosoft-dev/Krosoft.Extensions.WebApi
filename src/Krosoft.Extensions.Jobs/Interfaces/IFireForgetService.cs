namespace Krosoft.Extensions.Jobs.Interfaces;

public interface IFireForgetService
{
    void Fire<T>(Action<T> action) where T : notnull;
    void FireAsync<T>(Func<T, Task> func) where T : notnull;
}