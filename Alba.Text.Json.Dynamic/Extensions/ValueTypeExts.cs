using System.Text.Json;
using System.Text.Json.Nodes;
using Alba.Framework;

namespace Alba.Text.Json.Dynamic.Extensions;

public static class ValueTypeExts
{
    private static readonly JsonValue NoJsonValue = Ensure.NotNull(JsonValue.Create("<NOVALUE>"));

    extension<T>(T @this)
    {
        public JsonNode? ToJsonNode(JsonNodeOptions? options = null) =>
            @this.ToJsonValue(out var valueNode, options)
                ? valueNode
                : @this switch {
                    // already JNode, clone if used within another tree
                    JNode { NodeUntyped: var v } => v.Parent == null ? v : v.DeepClone(),
                    // already JsonNode
                    JsonNode v => v,
                    // element is always stored as JsonValueOfElement
                    JsonElement v => JsonValue.Create(v, options),
                    // serialize everything else into node
                    JsonDocument v => JsonSerializer.SerializeToNode(v),
                    _ => JsonSerializer.SerializeToNode(@this),
                };

        public bool ToJsonValue([NotNullIfNotNull(nameof(@this))] out JsonValue? valueNode, JsonNodeOptions? options = null)
        {
            valueNode = @this switch {
                null => null,
                // already JsonValue
                JsonValue v => v,
                // types stored as JsonValuePrimitive<T> (explicit ctor)
                bool v => JsonValue.Create(v, options),
                char v => JsonValue.Create(v, options),
                sbyte v => JsonValue.Create(v, options),
                byte v => JsonValue.Create(v, options),
                short v => JsonValue.Create(v, options),
                ushort v => JsonValue.Create(v, options),
                int v => JsonValue.Create(v, options),
                uint v => JsonValue.Create(v, options),
                long v => JsonValue.Create(v, options),
                ulong v => JsonValue.Create(v, options),
                float v => JsonValue.Create(v, options),
                double v => JsonValue.Create(v, options),
                decimal v => JsonValue.Create(v, options),
                DateTime v => JsonValue.Create(v, options),
                DateTimeOffset v => JsonValue.Create(v, options),
                Guid v => JsonValue.Create(v, options),
                // types stored as JsonValuePrimitive<T> (no explicit ctor)
                TimeSpan or Uri or Version => JsonValue.Create(@this, options),
              #if NET5_0_OR_GREATER
                Half => JsonValue.Create(@this, options),
              #endif
              #if NET6_0_OR_GREATER
                DateOnly or TimeOnly => JsonValue.Create(@this, options),
              #endif
              #if NET7_0_OR_GREATER
                Int128 or UInt128 => JsonValue.Create(@this, options),
              #endif
                // convert to JsonValueOfElement if it's a value
                JsonElement el => el.ValueKind is not (JsonValueKind.Object or JsonValueKind.Array)
                    ? JsonValue.Create(el, options)
                    : NoJsonValue,
                // everything else isn't a value
                _ => NoJsonValue,
            };
            return !ReferenceEquals(valueNode, NoJsonValue);
        }
    }
}