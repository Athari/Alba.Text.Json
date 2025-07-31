using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

internal static class JOperations
{
    [field: ThreadStatic, MaybeNull]
    private static Utf8NumberWriter NumberWriter => field ??= new(256);

    public static readonly MethodRef JsonObject_GetMember_Method = MethodRef.Of(() =>
        JsonObject_GetMember(null!, null!));

    public static readonly MethodRef JsonObject_SetMember_Method = MethodRef.Of(() =>
        JsonObject_SetMember(null!, null!, MethodKey.GetT<string>(0)));

    public static readonly MethodRef JsonArray_GetIndex_Method = MethodRef.Of(() =>
        JsonArray_GetIndex(null!, 0));

    public static readonly MethodRef JsonArray_SetIndex_Method = MethodRef.Of(() =>
        JsonArray_SetIndex(null!, 0, MethodKey.GetT<string>(0)));

    public static object? JsonObject_GetMember(JObject d, string name) =>
        NodeToDynamicNode(d.Node[name], d.Options);

    public static object? JsonObject_SetMember<T>(JObject d, string name, T value) =>
        (d.Node[name] = ValueToNode(value, d.Node.Options), value).value;

    public static object? JsonArray_GetIndex(JArray d, int index) =>
        NodeToDynamicNode(d.Node[index], d.Options);

    public static object? JsonArray_SetIndex<T>(JArray d, int index, T value) =>
        (d.Node[index] = ValueToNode(value, d.Node.Options), value).value;

    private static JsonNode? ValueToNode<T>(T value, JsonNodeOptions? opts)
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

    public static object? NodeToDynamicNode(JsonNode? node, JNodeOptions options)
    {
        object? o = node switch {
            null => null,
            // return primitive values directly
            JsonValue v => v.GetValueKind() switch {
                JsonValueKind.Null or JsonValueKind.Undefined => null,
                JsonValueKind.String => v.GetValue<string>(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                // Numbers can be stored as JsonElement pointing to barely parsed binary data
                JsonValueKind.Number => v.GetValue<object>() switch {
                    JsonElement j => JsonElementToNumber(j, options),
                    { } n => n,
                },
                // TODO Figure out what to do with raw objects inside JsonValueCustomized<T>
                JsonValueKind.Object or JsonValueKind.Array => v.GetValue<object>(),
                _ => throw new InvalidOperationException($"Unexpected JsonValueKind: {v}"),
            },
            // wrap with dynamic wrappers
            JsonObject v => new JObject(v, options),
            JsonArray v => new JArray(v, options),
            _ => throw new InvalidOperationException($"Unexpected JsonNode: {node.GetType()}"),
        };
        //WriteLine($"{d.GetType().Name}: {node?.GetType().Name} => {o?.GetType().Name}");
        return o;
    }

    private static object JsonElementToNumber(JsonElement j, JNodeOptions options)
    {
        var writer = NumberWriter;
        writer.CopyFrom(j);
        return writer.DetectFloatingPoint()
            ? JsonElementToNumberWithAnyTypeCode(j, options.FloatTypes)
         ?? throw new InvalidOperationException($"Cannot convert {j} to a floating point number.")
            : JsonElementToNumberWithAnyTypeCode(j, options.IntegerTypes)
         ?? throw new InvalidOperationException($"Cannot convert {j} to an integer number.");
    }

    private static object? JsonElementToNumberWithAnyTypeCode(JsonElement j, TypeCode[] types)
    {
        foreach (var type in types)
            if (JsonElementToNumberWithTypeCode(j, type) is { } ret)
                return ret;
        return null;
    }

    private static object? JsonElementToNumberWithTypeCode(JsonElement j, TypeCode type)
    {
        return type switch {
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
    }

    private class Utf8NumberWriter
    {
        private static readonly JsonWriterOptions JsonWriterOptions = new() {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Indented = false,
            SkipValidation = true,
        };

        private readonly FixedArrayBufferWriter<byte> _buffer;
        private readonly Utf8JsonWriter _writer;

        public Utf8NumberWriter(int size)
        {
            _buffer = new(size);
            _writer = new(_buffer, JsonWriterOptions);
        }

        public void CopyFrom(JsonElement j)
        {
            _buffer.ResetIndex();
            j.WriteTo(_writer);
        }

        public bool DetectFloatingPoint()
        {
            // match \. | [Ee] \-
            var span = _buffer.WrittenSpan;
            for (int i = 0; i < span.Length; i++) {
                byte b = span[i];
                if (b == '.' || ((b == 'e' || b == 'E') && i + 1 < span.Length && span[i + 1] == '-'))
                    return true;
            }
            return false;
        }
    }
}