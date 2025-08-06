using System.Text.Json;
using System.Text.Json.Nodes;
#if NET5_0_OR_GREATER && !NET8_0_OR_GREATER
using System.Globalization;
#endif

namespace Alba.Text.Json.Dynamic;

internal static partial class JOperations
{
    private static readonly JsonValue NoJsonValue = Ensure.NotNull(JsonValue.Create("<NOVALUE>"));

    public static JsonNode? ValueToNewJsonNode<T>(T value, JsonNodeOptions? opts) =>
        ValueToJsonValueNode(value, out var valueNode, opts)
            ? valueNode
            : value switch {
                // already JNode, clone if used within another tree
                JNode { NodeUntyped: var v } => v.Parent == null ? v : JsonNodeDeepClone(v),
                // already JsonNode
                JsonNode v => v,
                // element is always stored as JsonValueOfElement
                JsonElement v => JsonValue.Create(v, opts),
                // serialize everything else into node
                JsonDocument v => JsonSerializer.SerializeToNode(v),
                _ => JsonSerializer.SerializeToNode(value),
            };

    private static bool ValueToJsonValueNode<T>(T value,
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

    [return: NotNullIfNotNull(nameof(node))]
    public static object? JsonNodeToJNodeOrValue(JsonNode? node, JNodeOptions options) =>
        node switch {
            null => null,
            JsonValue v => JsonValueToValue(v, options),
            JsonObject v => new JObject(v, options),
            JsonArray v => new JArray(v, options),
            _ => throw new InvalidOperationException($"Unexpected JsonNode: {node.GetType()}"),
        };

    private static object? JsonValueToValue(JsonValue v, JNodeOptions options) =>
        v.GetValueKind() switch {
            // return primitive values directly
            JsonValueKind.Undefined => options.UndefinedValue,
            JsonValueKind.Null => null,
            JsonValueKind.String => v.GetValue<string>(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            // numbers can be stored as JsonElement pointing to barely parsed binary data
            JsonValueKind.Number => v.TryGetElementValue(out var el) ? JsonElementToNumber(el, options) : v.GetValue<object>(),
            // objects can be JsonElement or an arbitrary user type
            JsonValueKind.Object or JsonValueKind.Array
                or (JsonValueKind)byte.MaxValue => // from JsonNodeExts
                v.TryGetElementValue(out var el)
                    // return value of wrapped JsonElement
                    ? JsonElementToValue(el, options)
                    // return raw objects stored inside
                    : v.GetValue<object>(),
            _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonNode: {v}"),
        };

    private static object? JsonElementToValue(in JsonElement el, JNodeOptions options) =>
        el.ValueKind switch {
            JsonValueKind.Undefined => options.UndefinedValue,
            JsonValueKind.Null => null,
            JsonValueKind.String => el.GetString(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Number => JsonElementToNumber(el, options),
            JsonValueKind.Object or JsonValueKind.Array =>
                throw new InvalidOperationException($"{el.ValueKind} JsonElement in JsonValue"),
            _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {el.ValueKind}"),
        };

    private static object JsonElementToNumber(in JsonElement el, JNodeOptions options) =>
        IsFloatingPoint(el.RawValueSpan) ?
            JsonElementToNumberType(el, options.FloatTypes) ??
            throw new InvalidOperationException($"Cannot convert {el} to a floating point number.") :
            JsonElementToNumberType(el, options.IntegerTypes) ??
            throw new InvalidOperationException($"Cannot convert {el} to an integer number.");

    private static object? JsonElementToNumberType(JsonElement el, NumberType[] types) =>
        types.Select(t => JsonElementToNumberType(el, t)).FirstOrDefault(v => v != null);

    [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", Justification = "Intentional")]
    private static object? JsonElementToNumberType(in JsonElement el, NumberType type) =>
        type switch {
            NumberType.SByte => el.TryGetSByte(out var v) ? v : null,
            NumberType.Byte => el.TryGetByte(out var v) ? v : null,
            NumberType.Int16 => el.TryGetInt16(out var v) ? v : null,
            NumberType.UInt16 => el.TryGetUInt16(out var v) ? v : null,
            NumberType.Int32 => el.TryGetInt32(out var v) ? v : null,
            NumberType.UInt32 => el.TryGetUInt32(out var v) ? v : null,
            NumberType.Int64 => el.TryGetInt64(out var v) ? v : null,
            NumberType.UInt64 => el.TryGetUInt64(out var v) ? v : null,
            NumberType.Single => el.TryGetSingle(out var v) ? v : null,
            NumberType.Double => el.TryGetDouble(out var v) ? v : null,
            NumberType.Decimal => el.TryGetDecimal(out var v) ? v : null,
          #if NET8_0_OR_GREATER
            NumberType.Half => Half.TryParse(el.RawValueSpan, Invariant, out var v) ? v : null,
          #elif NET5_0_OR_GREATER
            NumberType.Half => Half.TryParse(el.RawText, NumberStyles.Float | NumberStyles.AllowThousands, Invariant, out var v) ? v : null,
          #endif
          #if NET8_0_OR_GREATER
            NumberType.Int128 => Int128.TryParse(el.RawValueSpan, Invariant, out var v) ? v : null,
            NumberType.UInt128 => UInt128.TryParse(el.RawValueSpan, Invariant, out var v) ? v : null,
          #elif NET7_0_OR_GREATER
            NumberType.Int128 => Int128.TryParse(el.RawText, Invariant, out var v) ? v : null,
            NumberType.UInt128 => UInt128.TryParse(el.RawText, Invariant, out var v) ? v : null,
          #endif
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"A number or string NumberKind expected, got {type}"),
        };

    private static bool IsFloatingPoint(ReadOnlySpan<byte> span) =>
        span.IndexOf((byte)'.') switch {
            -1 => span.IndexOfAny((byte)'e', (byte)'E') switch {
                -1 => false,
                var i => i + 1 < span.Length && span[i + 1] == (byte)'-',
            },
            _ => true,
        };

    public static bool JsonNodeEqualsJsonElement(JsonNode? n1, JsonElement el2, Equality equality, JNodeOptions options)
    {
        var el1null = n1 == null || JsonNodeIsNull(n1, options);
        var el2null = JsonElementIsNull(el2, options);
        if (el1null && el2null)
            return true;
        else if (el1null || el2null)
            return false;

        var n1n = n1!;
        if (n1n.TryGetElementValue(out var el1))
            return JsonElementEquals(el1, el2, equality, options);

        // Let's hope non-element JsonValue isn't compared with JsonElement often and just deserialize
        var doc1 = n1n.Deserialize<JsonDocument>();
        if (doc1?.RootElement != null)
            return JsonElementEquals(doc1.RootElement, el2, equality, options);

        // Can doc be null here?
        return el2null;
    }

    public static int JsonNodeListIndexOf<T>(IEnumerable<JsonNode?> nodes, T value, JNodeOptions options)
    {
        return value switch {
            JsonNode node => IndexOfJsonNode(node),
            JNode node => IndexOfJsonNode(node.NodeUntyped),
            JsonElement el => IndexOfJsonElement(el),
            JsonDocument doc => IndexOfJsonElement(doc.RootElement),
            _ => ValueToJsonValueNode(value, out var node, options.JsonNodeOptions)
                ? IndexOfJsonNode(node) : -1,
        };
        int IndexOfJsonNode(JsonNode? node) =>
            nodes.IndexOf(n => JsonNodeEquals(n, node, options.SearchEquality, options));
        int IndexOfJsonElement(JsonElement el) =>
            nodes.IndexOf(n => JsonNodeEqualsJsonElement(n, el, options.SearchEquality, options));
    }

    public static bool JsonNodeListContains<T>(IEnumerable<JsonNode?> nodes, T value, JNodeOptions options) =>
        JsonNodeListIndexOf(nodes, value, options) != -1;

    public static bool JsonNodeListRemove<T>(IList<JsonNode?> nodes, T value, JNodeOptions options)
    {
        var index = JsonNodeListIndexOf(nodes, value, options);
        if (index == -1)
            return false;
        nodes.RemoveAt(index);
        return true;
    }
}