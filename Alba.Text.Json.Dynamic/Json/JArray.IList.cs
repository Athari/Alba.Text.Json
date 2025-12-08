using System.Collections;
using System.Text.Json.Nodes;
using Alba.Framework;
using Alba.Text.Json.Dynamic.Extensions;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JArray
    : IList<object?>, IReadOnlyList<object?>
{
    bool ICollection<object?>.IsReadOnly => false;

    void ICollection<object?>.CopyTo(object?[] array, int index)
    {
        Ensure.Count(array, Count + index);
        int i = index;
        foreach (var item in Node)
            array[i++] = JsonNode.ToJNodeOrValue(item, Options);
    }

    void ICollection<object?>.Add(object? item) =>
        Add(item);

    void IList<object?>.Insert(int index, object? item) =>
        Insert(index, item);

    bool ICollection<object?>.Remove(object? item) =>
        Remove(item);

    bool ICollection<object?>.Contains(object? item) =>
        Contains(item);

    int IList<object?>.IndexOf(object? item) =>
        IndexOf(item);

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}