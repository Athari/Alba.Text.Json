using System.Text.Json;
using System.Text.Json.Nodes;
using Alba.Framework;

namespace Alba.Text.Json.Dynamic.Extensions;

public static class ValueTypeExts
{
    private static readonly JsonValue NoJsonValue = Ensure.NotNull(JsonValue.Create("<NOVALUE>"));

    public static JsonNode? ToNewJsonNode<T>(T value, JsonNodeOptions? opts) =>
        ToJsonValueNode(value, out var valueNode, opts)
            ? valueNode
            : value switch {
                // already JNode, clone if used within another tree
                JNode { NodeUntyped: var v } => v.Parent == null ? v : JsonNode.DeepClone(v),
                // already JsonNode
                JsonNode v => v,
                // element is always stored as JsonValueOfElement
                JsonElement v => JsonValue.Create(v, opts),
                // serialize everything else into node
                JsonDocument v => JsonSerializer.SerializeToNode(v),
                _ => JsonSerializer.SerializeToNode(value),
            };

    public static bool ToJsonValueNode<T>(T value,
        [NotNullIfNotNull(nameof(value))] out JsonValue? valueNode, JsonNodeOptions? opts)
    {
        valueNode = value switch {
            null => null,
            // already JsonValue
            JsonValue v => v,
            // types stored as JsonValuePrimitive<T> (explicit ctor)
            bool v => JsonValue.Create(v, opts),
            char v => JsonValue.Create(v, opts),
            sbyte v => JsonValue.Create(v, opts),
            byte v => JsonValue.Create(v, opts),
            short v => JsonValue.Create(v, opts),
            ushort v => JsonValue.Create(v, opts),
            int v => JsonValue.Create(v, opts),
            uint v => JsonValue.Create(v, opts),
            long v => JsonValue.Create(v, opts),
            ulong v => JsonValue.Create(v, opts),
            float v => JsonValue.Create(v, opts),
            double v => JsonValue.Create(v, opts),
            decimal v => JsonValue.Create(v, opts),
            DateTime v => JsonValue.Create(v, opts),
            DateTimeOffset v => JsonValue.Create(v, opts),
            Guid v => JsonValue.Create(v, opts),
            // types stored as JsonValuePrimitive<T> (no explicit ctor)
            TimeSpan or Uri or Version => JsonValue.Create(value, opts),
          #if NET5_0_OR_GREATER
            Half => JsonValue.Create(value, opts),
          #endif
          #if NET6_0_OR_GREATER
            DateOnly or TimeOnly => JsonValue.Create(value, opts),
          #endif
          #if NET7_0_OR_GREATER
            Int128 or UInt128 => JsonValue.Create(value, opts),
          #endif
            // convert to JsonValueOfElement if it's a value
            JsonElement el => el.ValueKind is not (JsonValueKind.Object or JsonValueKind.Array)
                ? JsonValue.Create(el, opts)
                : NoJsonValue,
            // everything else isn't a value
            _ => NoJsonValue,
        };
        return !ReferenceEquals(valueNode, NoJsonValue);
    }
}