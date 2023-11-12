namespace Krosoft.Extensions.Core.Interfaces;

public interface IJsonService
{
    string Serialize<T>(T obj);
    T? Deserialize<T>(string jsonString);
}