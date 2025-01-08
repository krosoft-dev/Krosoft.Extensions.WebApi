namespace Krosoft.Extensions.Testing.Models;

internal record DataReaderDescriptor
{
    private readonly string[] _properties;

    public DataReaderDescriptor(IEnumerable<IEnumerable<KeyValuePair<string, object?>>> data)
    {
        Data = data;
        Closed = false;

        _properties = Data.SelectMany(x => x.Select(y => y.Key)).Distinct().ToArray();
    }

    public IEnumerable<IEnumerable<KeyValuePair<string, object?>>> Data { get; }
    public int FieldCount => _properties.Length;
    public bool Closed { get; set; }

    public string GetName(int i)
    {
        ThrowIfOutOfRange(i);

        return _properties[i];
    }

    public int GetOrdinal(string name)
    {
        for (var i = 0; i < _properties.Length; ++i)
        {
            if (_properties[i] == name)
            {
                return i;
            }
        }

        throw new IndexOutOfRangeException();
    }

    public TValue? GetValue<TValue>(int r, int c)
    {
        ThrowIfOutOfRange(c);

        var prop = GetName(c);
        var row = Data.ToArray()[r];
        var keyValuePair = row.FirstOrDefault(x => x.Key == prop);

        return (TValue?)keyValuePair.Value;
    }

    public int GetValues(int r, object?[] values)
    {
        var length = Math.Min(FieldCount, values.Length);
        for (var c = 0; c < length; c++)
        {
            values[c] = GetValue<object?>(r, c);
        }

        return length;
    }

    private void ThrowIfOutOfRange(int c)
    {
        if (Closed)
        {
            throw new InvalidOperationException();
        }

        if (c < 0 || c >= _properties.Length)
        {
            throw new IndexOutOfRangeException();
        }
    }
}