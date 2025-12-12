using System.Text.Json;
using System.Text.Json.Nodes;
using Alba.Framework;
using Alba.Text.Json.Extensions;

namespace Alba.Text.Json.Dynamic;

internal static class JsonNodeCollectionExts
{
    extension(IEnumerable<JsonNode?> @this)
    {
        public int IndexOf<T>(T value, JNodeOptions options) =>
            value switch {
                JsonNode node => @this.IndexOf(node, options),
                JNode node => @this.IndexOf(node.NodeUntyped, options),
                JsonElement el => @this.IndexOf(el, options),
                JsonDocument doc => @this.IndexOf(doc.RootElement, options),
                _ => @this.IndexOf(value.ToJsonNode(options.JsonNodeOptions, false), options),
            };

        private int IndexOf(JsonNode? node, JNodeOptions options) =>
            @this.IndexOf(n => JsonNode.Equals(n, node, options.SearchEquality, options));

        private int IndexOf(JsonElement el, JNodeOptions options) =>
            @this.IndexOf(n => JsonNode.Equals(n, el, options.SearchEquality, options));

        public bool Contains<T>(T value, JNodeOptions options) =>
            IndexOf(@this, value, options) != -1;
    }

    extension(IList<JsonNode?> @this)
    {
        public bool Remove<T>(T value, JNodeOptions options)
        {
            var index = IndexOf(@this, value, options);
            if (index == -1)
                return false;
            @this.RemoveAt(index);
            return true;
        }
    }

    extension(IEnumerable<KeyValuePair<string, JsonNode?>> @this)
    {
        public int IndexOf<T>(KeyValuePair<string, T> property, JNodeOptions options) =>
            property.Value switch {
                JsonNode node => @this.IndexOf(property.Key, node, options),
                JNode node => @this.IndexOf(property.Key, node.NodeUntyped, options),
                JsonElement el => @this.IndexOf(property.Key, el, options),
                JsonDocument doc => @this.IndexOf(property.Key, doc.RootElement, options),
                _ => @this.IndexOf(property.Key, property.Value.ToJsonNode(options.JsonNodeOptions, false), options),
            };

        private int IndexOf(string propertyName, JsonNode? node, JNodeOptions options) =>
            @this.IndexOf(n =>
                options.ArePropertyNamesEqual(propertyName, n.Key) &&
                JsonNode.Equals(n.Value, node, options.SearchEquality, options));

        private int IndexOf(string propertyName, JsonElement el, JNodeOptions options) =>
            @this.IndexOf(n =>
                options.ArePropertyNamesEqual(propertyName, n.Key) &&
                JsonNode.Equals(n.Value, el, options.SearchEquality, options));

        public bool Contains<T>(KeyValuePair<string, T> property, JNodeOptions options) =>
            IndexOf(@this, property, options) != -1;
    }

    extension(IList<KeyValuePair<string, JsonNode?>> @this)
    {
        public bool Remove<T>(KeyValuePair<string, T?> value, JNodeOptions options)
        {
            var index = IndexOf(@this, value, options);
            if (index == -1)
                return false;
            @this.RemoveAt(index);
            return true;
        }
    }
}