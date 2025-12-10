using System.Collections;
using Alba.Framework;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JArray
    : IList<object?>, IReadOnlyList<object?>
{
    /// <summary>Returns <see langword="false"/>.</summary>
    bool ICollection<object?>.IsReadOnly => false;

    void ICollection<object?>.CopyTo(object?[] array, int index)
    {
        Ensure.Count(array, Count + index);
        int i = index;
        foreach (var item in Node)
            array[i++] = item.ToJNodeOrValue(Options);
    }

    /// <inheritdoc cref="Add"/>
    void ICollection<object?>.Add(object? item) =>
        Add(item);

    /// <inheritdoc cref="Insert{T}(int,T)"/>
    void IList<object?>.Insert(int index, object? item) =>
        Insert(index, item);

    /// <inheritdoc cref="Remove"/>
    bool ICollection<object?>.Remove(object? item) =>
        Remove(item);

    /// <inheritdoc cref="Contains"/>
    bool ICollection<object?>.Contains(object? item) =>
        Contains(item);

    /// <inheritdoc cref="IndexOf"/>
    int IList<object?>.IndexOf(object? item) =>
        IndexOf(item);

    /// <inheritdoc cref="GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}