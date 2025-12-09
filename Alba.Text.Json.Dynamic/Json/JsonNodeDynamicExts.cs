using System.Text.Json.Nodes;
using Alba.Text.Json.Dynamic.Extensions;

namespace Alba.Text.Json.Dynamic;

public static class JNodeDynamicExts
{
    extension(JsonNode? @this)
    {
        public dynamic? ToDynamic(JNodeOptions? options = null) =>
            @this.ToJNodeOrValue(options ?? JNodeOptions.Default);

        [return: NotNullIfNotNull(nameof(@this))]
        public object? ToJNodeOrValue(JNodeOptions options) =>
            @this switch {
                null => null,
                JsonValue v => v.ToValue(options),
                JsonObject v => new JObject(v, options),
                JsonArray v => new JArray(v, options),
                _ => throw new InvalidOperationException($"Unexpected JsonNode: {@this.GetType()}"),
            };
    }
}