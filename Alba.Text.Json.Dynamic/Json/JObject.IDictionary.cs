using System.Collections;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JObject
    : IDictionary<string, object?>, IReadOnlyDictionary<string, object?>
{
    private IDictionary<string, JsonNode?> NodeDictionary => Node;

    /// <summary>Gets a collection containing the property names in the <see cref="JObject"/>.</summary>
    private ICollection<string> Keys => NodeDictionary.Keys;

    /// <summary>Gets a collection containing the property values in the <see cref="JObject"/>.</summary>
    private ICollection<object?> Values => new ValueCollection(this);

    // IDictionary<string, JsonNode?>

    /// <inheritdoc cref="Keys"/>
    ICollection<string> IDictionary<string, object?>.Keys => Keys;

    /// <inheritdoc cref="Values"/>
    ICollection<object?> IDictionary<string, object?>.Values => Values;

    /// <inheritdoc cref="Add"/>
    void IDictionary<string, object?>.Add(string key, object? value) =>
        Add(key, value);

    /// <inheritdoc cref="TryGet"/>
    bool IDictionary<string, object?>.TryGetValue(string propertyName, out object? value) =>
        TryGet(propertyName, out value);

    // IReadOnlyDictionary<string, JsonNode?>

    /// <inheritdoc cref="Keys"/>
    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => Keys;

    /// <inheritdoc cref="Values"/>
    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => Values;

    /// <inheritdoc cref="TryGet"/>
    bool IReadOnlyDictionary<string, object?>.TryGetValue(string propertyName, out object? value) =>
        TryGet(propertyName, out value);

    // ICollection<KeyValuePair<string, object?>>

    /// <summary>Copies the elements of the <see cref="JObject"/> to an array of type KeyValuePair starting at the specified array index.</summary>
    /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref="JObject"/>.</param>
    /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.</exception>
    /// <exception cref="ArgumentException">The number of elements in the source ICollection is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.</exception>
    void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int index)
    {
        var i = index;
        foreach ((string name, var value) in Node)
            array[i++] = new(name, value.ToJNodeOrValue(Options));
    }

    /// <summary>Returns <see langword="false"/>.</summary>
    bool ICollection<KeyValuePair<string, object?>>.IsReadOnly => false;

    /// <inheritdoc cref="Add"/>
    void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item) =>
        Add(item.Key, item.Value);

    /// <summary>Determines whether the <see cref="JObject"/> contains a specific property name and <see cref="JsonNode"/> reference.</summary>
    /// <param name="item">The element to locate in the <see cref="JObject"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="JObject"/> contains an element with the property name; otherwise, <see langword="false"/>.</returns>
    bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item) =>
        TryGet(item.Key, out var value) && Equals(item.Value, value);

    /// <summary>Removes a key and value from the <see cref="JObject"/>.</summary>
    /// <param name="item">The KeyValuePair structure representing the property name and value to remove from the <see cref="JObject"/>.</param>
    /// <returns><see langword="true"/> if the element is successfully removed; otherwise, <see langword="false"/>.</returns>
    bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item) =>
        TryGet(item.Key, out var value) && Equals(item.Value, value) && Remove(item.Key);

    // IEnumerable

    /// <inheritdoc cref="GetEnumerator"/>
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
            source.Node.Select(p => p.Value).Contains(item, source.Options);

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