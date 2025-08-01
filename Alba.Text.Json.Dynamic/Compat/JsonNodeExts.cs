#if !JSON8_0_OR_GREATER

using C = System.TypeCode;

namespace System.Text.Json.Nodes;

internal static class JsonNodeExts
{
    public static JsonValueKind GetValueKind(this JsonNode? @this)
    {
        switch (@this) {
            case null:
                return JsonValueKind.Null;
            case JsonObject:
                return JsonValueKind.Object;
            case JsonArray:
                return JsonValueKind.Array;
            case JsonValue value: {
                if (value.TryGetValue(out JsonElement el))
                    return el.ValueKind;
                object obj = value.GetValue<object>();
                switch (Convert.GetTypeCode(obj)) {
                    case C.Empty:
                        return JsonValueKind.Null;
                    case C.Boolean:
                        return (bool)obj ? JsonValueKind.True : JsonValueKind.False;
                    case C.String:
                        return JsonValueKind.String;
                    case C.SByte or C.Byte
                        or C.Int16 or C.UInt16 or C.Int32 or C.UInt32 or C.Int64 or C.UInt64
                        or C.Single or C.Double or C.Decimal:
                        return JsonValueKind.Number;
                }
                switch (obj) {
                  #if NET5_0_OR_GREATER
                    case Half:
                        return JsonValueKind.Number;
                  #endif
                  #if NET7_0_OR_GREATER
                    case Int128 or UInt128:
                        return JsonValueKind.Number;
                  #endif
                    default:
                        break;
                }
                // Wrapped object or array
                return (JsonValueKind)byte.MaxValue;
            }
            default:
                throw new ArgumentException($"Unexpected JsonNode type: {@this.GetType().Name}");
        }
    }
}

#endif