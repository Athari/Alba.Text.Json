using System.Text.Json.Nodes;
using Alba.Text.Json.Extensions;

namespace Alba.Text.Json.Dynamic;

/// <summary>Extension methods for <see cref="JsonNode"/> related to <see langword="dynamic"/> type support.</summary>
public static class JNodeDynamicExts
{
    ///
    extension(JsonNode? @this)
    {
        /// <summary>Converts a <see cref="JsonNode"/> to a <see cref="JNode"/> or a primitive value, typed as <see langword="dynamic"/>.</summary>
        /// <param name="options">Options to control the behavior.</param>
        /// <returns>A <see langword="dynamic"/> adapter for <see cref="JsonNode"/> or a primitive value.</returns>
        public dynamic? ToDynamic(JNodeOptions? options = null) =>
            @this.ToJNodeOrValue(options ?? JNodeOptions.Default);

        /// <summary>Converts a <see cref="JsonNode"/> to a <see cref="JNode"/> or a primitive value.</summary>
        /// <param name="options">Options to control the behavior.</param>
        /// <returns>A <see langword="dynamic"/> adapter for <see cref="JsonNode"/> or a primitive value.</returns>
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