using JsonFlatFileDataStore;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Data.Json.Interfaces;
using Krosoft.Extensions.Data.Json.Models;
using Microsoft.Extensions.Options;

namespace Krosoft.Extensions.Data.Json.Services;

internal class JsonDataService<T> : IJsonDataService<T> where T : class
{
    private readonly JsonDataSettings _jsonDataSettings;

    public JsonDataService(IOptions<JsonDataSettings> options)
    {
        _jsonDataSettings = options.Value;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var collection = GetCollection();
        await collection.DeleteOneAsync(id);
    }

    public async Task InsertAsync(T item, CancellationToken cancellationToken)
    {
        var collection = GetCollection();
        await collection.InsertOneAsync(item);
    }

    public IEnumerable<T> Query()
    {
        var collection = GetCollection();
        return collection.AsQueryable();
    }

    public async Task UpdateAsync(int id, T item, CancellationToken cancellationToken)
    {
        var collection = GetCollection();
        await collection.UpdateOneAsync(id, item);
    }

    private IDocumentCollection<T> GetCollection()
    {
        if (string.IsNullOrEmpty(_jsonDataSettings.DataFileName))
        {
            throw new KrosoftTechniqueException($"{nameof(_jsonDataSettings.DataFileName)} non renseigné.");
        }

        var store = new DataStore(_jsonDataSettings.DataFileName);
        var collection = store.GetCollection<T>();
        return collection;
    }
}