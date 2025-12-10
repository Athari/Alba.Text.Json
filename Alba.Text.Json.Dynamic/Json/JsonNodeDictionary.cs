using System.Text.Json;
using System.Text.Json.Nodes;
using Alba.Framework;
using Alba.Text.Json.Dynamic.Extensions;

namespace Alba.Text.Json.Dynamic;

internal static class JsonNodeDictionary
{
    public static int IndexOf<T>(IEnumerable<KeyValuePair<string, JsonNode?>> properties,
        KeyValuePair<string, T> property, JNodeOptions options)
    {
        return property.Value switch {
            JsonNode node => IndexOfJsonNode(node),
            JNode node => IndexOfJsonNode(node.NodeUntyped),
            JsonElement el => IndexOfJsonElement(el),
            JsonDocument doc => IndexOfJsonElement(doc.RootElement),
            _ => property.ToJsonValue(out var node, options.JsonNodeOptions)
                ? IndexOfJsonNode(node) : -1,
        };
        int IndexOfJsonNode(JsonNode? node) =>
            properties.IndexOf(n =>
                options.PropertyNameEquals(property.Key, n.Key) &&
                JsonNode.Equals(n.Value, node, options.SearchEquality, options));
        int IndexOfJsonElement(JsonElement el) =>
            properties.IndexOf(n =>
                options.PropertyNameEquals(property.Key, n.Key) &&
                JsonNode.Equals(n.Value, el, options.SearchEquality, options));
    }

    public static bool Contains<T>(IEnumerable<KeyValuePair<string, JsonNode?>> properties,
        KeyValuePair<string, T> property, JNodeOptions options) =>
        IndexOf(properties, property, options) != -1;

    public static bool Remove<T>(IList<KeyValuePair<string, JsonNode?>> properties,
        KeyValuePair<string, T?> value, JNodeOptions options)
    {
        var index = IndexOf(properties, value, options);
        if (index == -1)
            return false;
        properties.RemoveAt(index);
        return true;
    }
}