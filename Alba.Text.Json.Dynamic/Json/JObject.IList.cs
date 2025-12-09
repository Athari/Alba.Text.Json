#if JSON9_0_OR_GREATER

using System.Text.Json.Nodes;
using Alba.Text.Json.Dynamic.Extensions;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JObject
    : IList<KeyValuePair<string, object?>>, IReadOnlyList<KeyValuePair<string, object?>>
{
    private IList<KeyValuePair<string, JsonNode?>> NodeList => Node;

    KeyValuePair<string, object?> IList<KeyValuePair<string, object?>>.this[int index] {
        get {
            var p = NodeList[index];
            return new(p.Key, p.Value.ToJNodeOrValue(Options));
        }
        set => NodeList[index] = new(value.Key, value.Value.ToJsonNode(Node.Options));
    }

    int IList<KeyValuePair<string, object?>>.IndexOf(KeyValuePair<string, object?> item) =>
        throw new NotImplementedException();

    void IList<KeyValuePair<string, object?>>.Insert(int index, KeyValuePair<string, object?> item) =>
        throw new NotImplementedException();

    void IList<KeyValuePair<string, object?>>.RemoveAt(int index) =>
        throw new NotImplementedException();

    KeyValuePair<string, object?> IReadOnlyList<KeyValuePair<string, object?>>.this[int index] =>
        throw new NotImplementedException();

    private partial class ValueCollection : IList<object?>, IReadOnlyList<object?>
    {
        public object? this[int index] {
            get => source.NodeList[index].Value.ToJNodeOrValue(source.Options);
            set => throw ReadOnly();
        }

        public int IndexOf(object? item) =>
            JsonNodeList.IndexOf(source.Node.Select(p => p.Value), item, source.Options);

        void IList<object?>.Insert(int index, object? item) => throw ReadOnly();
        void IList<object?>.RemoveAt(int index) => throw ReadOnly();
    }
}

#endif