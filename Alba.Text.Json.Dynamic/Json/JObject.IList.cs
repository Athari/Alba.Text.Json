#if JSON9_0_OR_GREATER

using System.Text.Json.Nodes;
using Alba.Text.Json.Dynamic.Extensions;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JObject
    : IList<KeyValuePair<string, object?>>, IReadOnlyList<KeyValuePair<string, object?>>
{
    private IList<KeyValuePair<string, JsonNode?>> NodeList => Node;

    /// <summary>Gets or sets the property the specified index. The assigned value is converted to <see cref="JNode"/> using <see cref="ValueTypeExts.ToJsonNode{T}"/>.</summary>
    /// <param name="index">The zero-based index of the pair to get or set.</param>
    /// <returns>The property at the specified index as a key/value pair.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.</exception>
    private KeyValuePair<string, object?> this[int index] {
        get {
            var property = NodeList[index];
            return new(property.Key, property.Value.ToJNodeOrValue(Options));
        }
        set => NodeList[index] = new(value.Key, value.Value.ToJsonNode(Node.Options));
    }

    // IList<KeyValuePair<string, object?>>

    /// <inheritdoc cref="this[int]"/>
    KeyValuePair<string, object?> IList<KeyValuePair<string, object?>>.this[int index] {
        get => this[index];
        set => this[index] = value;
    }

    /// <summary>Determines the index of a specific property key/value pair in the object. The value is searched using <see cref="JNodeOptions.SearchEquality"/> comparison kind.</summary>
    /// <param name="item">The property key/value pair to locate.</param>
    /// <exception cref="ArgumentNullException">Key of <paramref name="item"/> is null.</exception>
    int IList<KeyValuePair<string, object?>>.IndexOf(KeyValuePair<string, object?> item) =>
        NodeList.IndexOf(item, Options);

    /// <summary>Inserts a property into the object at the specified index. The value is converted to <see cref="JNode"/> using <see cref="ValueTypeExts.ToJsonNode{T}"/>.</summary>
    /// <param name="index">The zero-based index at which the property should be inserted.</param>
    /// <param name="item">The property key/value pair to insert.</param>
    /// <exception cref="ArgumentNullException">Key of <paramref name="item"/> is null.</exception>
    /// <exception cref="ArgumentException">An element with the same key already exists in the <see cref="JObject"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or greater than <see cref="Count"/>.</exception>
    void IList<KeyValuePair<string, object?>>.Insert(int index, KeyValuePair<string, object?> item) =>
        NodeList.Insert(index, new(item.Key, item.Value.ToJsonNode(Node.Options)));

    /// <summary>Removes the property at the specified index.</summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.</exception>
    void IList<KeyValuePair<string, object?>>.RemoveAt(int index) =>
        NodeList.RemoveAt(index);

    // IReadOnlyList<KeyValuePair<string, object?>>

    /// <inheritdoc cref="this[int]"/>
    KeyValuePair<string, object?> IReadOnlyList<KeyValuePair<string, object?>>.this[int index] =>
        this[index];

    private partial class ValueCollection : IList<object?>, IReadOnlyList<object?>
    {
        public object? this[int index] {
            get => source.NodeList[index].Value.ToJNodeOrValue(source.Options);
            set => throw ReadOnly();
        }

        public int IndexOf(object? item) =>
            source.Node.Select(p => p.Value).IndexOf(item, source.Options);

        void IList<object?>.Insert(int index, object? item) => throw ReadOnly();
        void IList<object?>.RemoveAt(int index) => throw ReadOnly();
    }
}

#endif