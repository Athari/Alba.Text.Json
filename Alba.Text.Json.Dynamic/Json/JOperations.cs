using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

internal static class JOperations
{
    private static readonly JsonValue NoJsonValue = JsonValue.Create("<NOVALUE>");

    public static bool ValueToValueNode<T>(T value, out JsonValue? valueNode, JsonNodeOptions? opts)
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
            JsonElement j => j.ValueKind is not (JsonValueKind.Object or JsonValueKind.Array)
                ? JsonValue.Create(j, opts)
                : NoJsonValue,
            // everything else isn't a value
            _ => NoJsonValue,
        };
        return !ReferenceEquals(valueNode, NoJsonValue);
    }

    public static JsonNode? ValueToNewNode<T>(T value, JsonNodeOptions? opts) =>
        ValueToValueNode(value, out var valueNode, opts)
            ? valueNode
            : value switch {
                // already JNode, clone if used within another tree
                JNode { NodeUntyped: var v } => v.Parent == null ? v : DeepClone(v),
                // already JsonNode
                JsonNode v => v,
                // element is always stored as JsonValueOfElement
                JsonElement j => JsonValue.Create(j, opts),
                // serialize everything else into node
                JsonDocument d => JsonSerializer.SerializeToNode(d),
                _ => JsonSerializer.SerializeToNode(value),
            };

    [return: NotNullIfNotNull(nameof(node))]
    public static object? NodeToDynamicNodeOrValue(JsonNode? node, JNodeOptions options) =>
        //WriteLine($"{d.GetType().Name}: {node?.GetType().Name} => {o?.GetType().Name}");
        node switch {
            null => null,
            JsonValue v => NodeToValue(v, options),
            JsonObject v => new JObject(v, options),
            JsonArray v => new JArray(v, options),
            _ => throw new InvalidOperationException($"Unexpected JsonNode: {node.GetType()}"),
        };

    private static object? NodeToValue(JsonValue v, JNodeOptions options) =>
        v.GetValueKind() switch {
            // return primitive values directly
            JsonValueKind.Undefined => options.UndefinedValue,
            JsonValueKind.Null => null,
            JsonValueKind.String => v.GetValue<string>(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            // numbers can be stored as JsonElement pointing to barely parsed binary data
            JsonValueKind.Number => v.GetValue<object>() switch {
                JsonElement j => JsonElementToNumber(j, options),
                { } n => n,
            },
            // objects can be JsonElement or an arbitrary user type
            JsonValueKind.Object or JsonValueKind.Array
                or (JsonValueKind)byte.MaxValue => // from JsonNodeExts
                v.TryGetValue(out JsonElement? j)
                    // return value of wrapped JsonElement
                    ? JsonElementToValue(j.Value, options)
                    // return raw objects stored inside
                    : v.GetValue<object>(),
            //v.TryGetValue<JsonElement>(out var el) ? el. v.GetValue<object>(),
            _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonNode: {v}"),
        };

    private static object? JsonElementToValue(JsonElement j, JNodeOptions options) =>
        j.ValueKind switch {
            JsonValueKind.Undefined => options.UndefinedValue,
            JsonValueKind.Null => null,
            JsonValueKind.String => j.GetString(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Number => JsonElementToNumber(j, options),
            JsonValueKind.Object or JsonValueKind.Array =>
                throw new InvalidOperationException($"{j.ValueKind} JsonElement in JsonValue"),
            _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {j.ValueKind}"),
        };

    private static object JsonElementToNumber(JsonElement j, JNodeOptions options) =>
        IsFloatingPoint(j.GetRawValueSpan()) ?
            JsonElementToNumberWithAnyTypeCode(j, options.FloatTypes) ??
            throw new InvalidOperationException($"Cannot convert {j} to a floating point number.") :
            JsonElementToNumberWithAnyTypeCode(j, options.IntegerTypes) ??
            throw new InvalidOperationException($"Cannot convert {j} to an integer number.");

    private static object? JsonElementToNumberWithAnyTypeCode(JsonElement j, TypeCode[] types)
    {
        foreach (var type in types)
            if (JsonElementToNumberWithTypeCode(j, type) is { } ret)
                return ret;
        return null;
    }

    [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", Justification = "Intentional")]
    private static object? JsonElementToNumberWithTypeCode(JsonElement j, TypeCode type) =>
        type switch {
            TypeCode.SByte => j.TryGetSByte(out var v) ? v : null,
            TypeCode.Byte => j.TryGetByte(out var v) ? v : null,
            TypeCode.Int16 => j.TryGetInt16(out var v) ? v : null,
            TypeCode.UInt16 => j.TryGetUInt16(out var v) ? v : null,
            TypeCode.Int32 => j.TryGetInt32(out var v) ? v : null,
            TypeCode.UInt32 => j.TryGetUInt32(out var v) ? v : null,
            TypeCode.Int64 => j.TryGetInt64(out var v) ? v : null,
            TypeCode.UInt64 => j.TryGetUInt64(out var v) ? v : null,
            TypeCode.Single => j.TryGetSingle(out var v) ? v : null,
            TypeCode.Double => j.TryGetDouble(out var v) ? v : null,
            TypeCode.Decimal => j.TryGetDecimal(out var v) ? v : null,
            TypeCode.String => j.GetString(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"A number or string TypeCode expected, got {type}"),
        };

    private static bool IsFloatingPoint(ReadOnlySpan<byte> span) =>
        span.IndexOf((byte)'.') switch {
            -1 => span.IndexOfAny((byte)'e', (byte)'E') switch {
                -1 => false,
                var i => i + 1 < span.Length && span[i + 1] == (byte)'-',
            },
            _ => true,
        };

    [return: NotNullIfNotNull(nameof(node))]
    public static JsonNode? DeepClone(JsonNode? node)
    {
      #if JSON8_0_OR_GREATER
        return node?.DeepClone();
      #else
        return node.Deserialize<JsonNode?>();
      #endif
    }

    public static bool DeepEquals(JsonNode? n1, JsonNode? n2, JNodeOptions options)
    {
      #if JSON8_0_OR_GREATER
        return JsonNode.DeepEquals(n1, n2);
      #else
        return (n1, n2) switch {
            (null, null) =>
                true,
            (JsonObject o1, JsonObject o2) =>
                o1.Count == o2.Count &&
                o1.Concat(o2)
                    .GroupBy(p => p.Key)
                    .Select(g => g.ToList())
                    .All(g => g.Count == 2 && DeepEquals(g[0].Value, g[1].Value, options)),
            (JsonArray a1, JsonArray a2) =>
                a1.Count == a2.Count &&
                a1.Zip(a2)
                    .All(p => DeepEquals(p.First, p.Second, options)),
            (JsonValue v1, JsonValue v2) =>
                ValueEquals(v1, v2, options),
            _ => false,
        };
      #endif
    }

    public static bool ShallowEquals(JsonNode? n1, JsonNode? n2, JNodeOptions options) =>
        (n1, n2) switch {
            (null, _) or (_, null) or (JsonArray, JsonArray) or (JsonObject, JsonObject) =>
                ReferenceEquals(n1, n2),
            (JsonValue v1, JsonValue v2) =>
                ValueEquals(v1, v2, options),
            _ => false,
        };

    public static bool ValueEquals(JsonValue va, JsonValue vb, JNodeOptions options)
    {
      #if JSON8_0_OR_GREATER
        return JsonNode.DeepEquals(va, vb);
      #else
        var (ka, kb) = (va.GetValueKind(), vb.GetValueKind());
        return ka == kb && ka switch {
            JsonValueKind.Null or JsonValueKind.Undefined or JsonValueKind.True or JsonValueKind.False =>
                true,
            JsonValueKind.String =>
                string.Equals(va.GetValue<string>(), vb.GetValue<string>(), StringComparison.Ordinal),
            JsonValueKind.Number =>
                Equals(NodeToValue(va, options), NodeToValue(vb, options)),
            _ =>
                throw new InvalidOperationException($"Unexpected JsonValueKind of JsonNode: {ka}"),
        };
      #endif
    }

    public static int GetDeepHashCode(this JsonNode node, int maxCount = int.MaxValue, int maxDepth = int.MaxValue)
    {
        var hash = 0;
        var count = 0;
        GetHashCodeProc(node, depth: 0);
        return hash;

        bool Add(object? value)
        {
            if (count >= maxCount)
                return false;
            else
                count++;
            hash = unchecked(hash * 397 ^ (value?.GetHashCode() ?? 0));
            return true;
        }

        void GetHashCodeProc(JsonNode? n, int depth)
        {
            if (n == null || !Add(n.GetType()))
                return;
            switch (n) {
                case JsonArray a:
                    if (depth < maxDepth)
                        foreach (var item in a)
                            GetHashCodeProc(item, depth + 1);
                    else
                        Add(a.Count);
                    break;
                case JsonObject o:
                    foreach (var (name, value) in o.OrderBy(p => p.Key, StringComparer.Ordinal)) {
                        if (!Add(name))
                            return;
                        if (depth < maxDepth)
                            GetHashCodeProc(value, depth + 1);
                    }
                    break;
                case JsonValue value:
                    Add(NodeToValue(value, JNodeOptions.Default));
                    break;
            }
        }
    }
}