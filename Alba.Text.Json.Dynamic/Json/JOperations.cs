using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

internal static class JOperations
{
    public static readonly MethodRef JsonObject_GetMember_Method = MethodRef.Of(() =>
        JsonObject_GetMember(null!, null!));

    public static readonly MethodRef JsonObject_SetMember_Method = MethodRef.Of(() =>
        JsonObject_SetMember(null!, null!, MethodKey.GetT<string>(0)));

    public static object? JsonObject_GetMember(JObject d, string name) =>
        NodeToDynamicNodeOrValue(d.Node[name], d.Options);

    public static object? JsonObject_SetMember<T>(JObject d, string name, T value) =>
        (d.Node[name] = ValueToNode(value, d.Node.Options), value).value;

    public static JsonNode? ValueToNode<T>(T value, JsonNodeOptions? opts)
    {
        return value switch {
            null => null,
            // already JsonNode
            JsonNode v => v,
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
            // element is always stored as JsonValueOfElement
            JsonElement => JsonValue.Create(value, opts),
            // serialize everything else into node
            JsonDocument => JsonSerializer.SerializeToNode(value),
            _ => JsonSerializer.SerializeToNode(value),
        };
    }

    public static object? NodeToDynamicNodeOrValue(JsonNode? node, JNodeOptions options) =>
        //WriteLine($"{d.GetType().Name}: {node?.GetType().Name} => {o?.GetType().Name}");
        node switch {
            null => null,
            JsonValue v => v.GetValueKind() switch {
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
            },
            // wrap with dynamic wrappers
            JsonObject v => new JObject(v, options),
            JsonArray v => new JArray(v, options),
            _ => throw new InvalidOperationException($"Unexpected JsonNode: {node.GetType()}"),
        };

    private static object? JsonElementToValue(in JsonElement j, JNodeOptions options) =>
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
}