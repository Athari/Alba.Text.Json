using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

internal static class JsonNodeList
{
    public static int IndexOf<T>(IEnumerable<JsonNode?> nodes, T value, JNodeOptions options)
    {
        return value switch {
            JsonNode node => IndexOfJsonNode(node),
            JNode node => IndexOfJsonNode(node.NodeUntyped),
            JsonElement el => IndexOfJsonElement(el),
            JsonDocument doc => IndexOfJsonElement(doc.RootElement),
            _ => JOperations.ValueToJsonValueNode(value, out var node, options.JsonNodeOptions)
                ? IndexOfJsonNode(node) : -1,
        };
        int IndexOfJsonNode(JsonNode? node) =>
            nodes.IndexOf(n => JsonNode.Equals(n, node, options.SearchEquality, options));
        int IndexOfJsonElement(JsonElement el) =>
            nodes.IndexOf(n => JsonNode.EqualsJsonElement(n, el, options.SearchEquality, options));
    }

    public static bool Contains<T>(IEnumerable<JsonNode?> nodes, T value, JNodeOptions options) =>
        IndexOf(nodes, value, options) != -1;

    public static bool Remove<T>(IList<JsonNode?> nodes, T value, JNodeOptions options)
    {
        var index = IndexOf(nodes, value, options);
        if (index == -1)
            return false;
        nodes.RemoveAt(index);
        return true;
    }
}