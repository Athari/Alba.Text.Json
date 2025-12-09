using System.Collections;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JObject
    : IDictionary<string, object?>, IReadOnlyDictionary<string, object?>
{
    private IDictionary<string, JsonNode?> NodeDictionary => Node;

    private ICollection<string> Keys => NodeDictionary.Keys;

    private ICollection<object?> Values => new ValueCollection(this);

    // IDictionary<string, JsonNode?>

    ICollection<string> IDictionary<string, object?>.Keys => Keys;

    ICollection<object?> IDictionary<string, object?>.Values => Values;

    void IDictionary<string, object?>.Add(string key, object? value) =>
        Add(key, value);

    bool IDictionary<string, object?>.TryGetValue(string key, out object? value) =>
        TryGet(key, out value);

    // IReadOnlyDictionary<string, JsonNode?>

    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => Keys;

    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => Values;

    bool IReadOnlyDictionary<string, object?>.TryGetValue(string key, out object? value) =>
        TryGet(key, out value);

    // ICollection<KeyValuePair<string, object?>>

    void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int index)
    {
        var i = index;
        foreach ((string name, var value) in Node)
            array[i++] = new(name, value.ToJNodeOrValue(Options));
    }

    bool ICollection<KeyValuePair<string, object?>>.IsReadOnly => false;

    void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item) =>
        Add(item.Key, item.Value);

    bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item) =>
        TryGet(item.Key, out var value) && Equals(item.Value, value);

    bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item) =>
        TryGet(item.Key, out var value) && Equals(item.Value, value) && Remove(item.Key);

    // IEnumerable

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

  #if JSON9_0_OR_GREATER
    private partial class ValueCollection(JObject source)
  #else
    private class ValueCollection(JObject source) : ICollection<object?>
  #endif
    {
        public int Count => source.Count;
        public bool IsReadOnly => true;

        public bool Contains(object? item) =>
            JsonNodeList.Contains(source.Node.Select(p => p.Value), item, source.Options);

        public IEnumerator<object?> GetEnumerator() =>
            source.Select(p => p.Value).GetEnumerator();

        // ICollection<object?>

        void ICollection<object?>.CopyTo(object?[] array, int index)
        {
            var i = index;
            foreach (var (_, value) in source.Node)
                array[i++] = value.ToJNodeOrValue(source.Options);
        }

        void ICollection<object?>.Add(object? item) => throw ReadOnly();
        void ICollection<object?>.Clear() => throw ReadOnly();
        bool ICollection<object?>.Remove(object? item) => throw ReadOnly();

        // IEnumerable

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        private static NotSupportedException ReadOnly() => new("Value collection is read-only.");
    }
}