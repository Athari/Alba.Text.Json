using System.Runtime.InteropServices;
using System.Text.Json;
#if NET5_0_OR_GREATER
using System.Globalization;
#endif
#if !JSON9_0_OR_GREATER
using System.Text.Encodings.Web;
using Alba.Framework;
#endif

namespace Alba.Text.Json.Dynamic.Extensions;

[SuppressMessage("Naming", "CA1708: Identifiers should differ by more than case", Justification = "Compiler bug")]
public static class JsonElementExts
{
  #if NET5_0_OR_GREATER
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;
  #endif

    extension(in JsonElement @this)
    {
        public string RawText => @this.GetRawText();

        public ReadOnlySpan<byte> RawValueSpan {
            get {
              #if JSON9_0_OR_GREATER
                return JsonMarshal.GetRawUtf8Value(@this);
              #else
                var writer = JsonElementWriter;
                writer.CopyFrom(@this);
                return writer.WrittenSpan;
              #endif
            }
        }

        public static object? ToValue(in JsonElement el, JNodeOptions options) =>
            el.ValueKind switch {
                JsonValueKind.Undefined => options.UndefinedValue,
                JsonValueKind.Null => null,
                JsonValueKind.String => el.GetString(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Number => JsonElement.ToNumber(el, options),
                JsonValueKind.Object or JsonValueKind.Array =>
                    throw new InvalidOperationException($"{el.ValueKind} JsonElement in JsonValue"),
                _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {el.ValueKind}"),
            };

        public static object ToNumber(in JsonElement el, JNodeOptions options) =>
            IsFloatingPoint(el.RawValueSpan) ?
                JsonElement.ToNumberType(el, options.FloatTypes) ??
                throw new InvalidOperationException($"Cannot convert {el} to a floating point number.") :
                JsonElement.ToNumberType(el, options.IntegerTypes) ??
                throw new InvalidOperationException($"Cannot convert {el} to an integer number.");

        private static object? ToNumberType(JsonElement el, NumberType[] types) =>
            types.Select(t => JsonElement.ToNumberType(el, t)).FirstOrDefault(v => v != null);

        [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", Justification = "Intentional")]
        private static object? ToNumberType(in JsonElement el, NumberType type) =>
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

        public static bool Equals(in JsonElement el1, in JsonElement el2, Equality equality, JNodeOptions options) =>
            equality switch {
                Equality.Deep => JsonElement.DeepEquals(el1, el2, options),
                Equality.Shallow => JsonElement.ShallowEquals(el1, el2, options),
                Equality.Reference => JsonElement.ReferenceEquals(el1, el2, options),
                _ => throw new ArgumentOutOfRangeException(nameof(equality), equality, null),
            };

        [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = "Conditional")]
        private static bool DeepEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options)
        {
            if (JsonElement.ReferenceEquals(el1, el2, options))
                return true;
          #if JSON9_0_OR_GREATER
            return JsonElement.DeepEquals(el1, el2);
          #else
            var (k1, k2) = (el1.ValueKind, el2.ValueKind);
            if (k1 != k2)
                return false;
            switch (k1) {
                case JsonValueKind.Object: {
                    using var it1 = el1.EnumerateObject();
                    using var it2 = el2.EnumerateObject();
                    while (it1.MoveNext()) {
                        if (!it2.MoveNext())
                            return false;
                        var (prop1, prop2) = (it1.Current, it2.Current);
                        if (!prop1.NameEquals(prop2.Name))
                            return UnorderedObjectDeepEquals(it1, it2, options);
                        if (!JsonElement.DeepEquals(prop1.Value, prop2.Value, options))
                            return false;
                    }
                    return !it2.MoveNext();
                }
                case JsonValueKind.Array: {
                    if (el1.GetArrayLength() != el2.GetArrayLength())
                        return false;
                    using var it1 = el1.EnumerateArray();
                    using var it2 = el2.EnumerateArray();
                    while (it1.MoveNext()) {
                        if (!it2.MoveNext() ||
                            !JsonElement.DeepEquals(it1.Current, it2.Current, options))
                            return false;
                    }
                    return !it2.MoveNext();
                }
                default:
                    return JsonElement.ValueEquals(el1, el2, options);
            }

            static bool UnorderedObjectDeepEquals(ObjectEnumerator it1, ObjectEnumerator it2, JNodeOptions options)
            {
                var d2 = new Dictionary<string, ValueQueue<JsonElement>>(StringComparer.Ordinal);
              #if NET6_0_OR_GREATER
                do {
                    d2.GetRefOrAdd(it2.Name).Enqueue(it2.Value);
                } while (it2.MoveNext());
                do {
                    ref var values = ref d2.GetRefOrNull(it1.Name, out var exists);
                    if (!exists ||
                        !values.TryDequeue(out var value) ||
                        !JsonElement.DeepEquals(it1.Value, value, options))
                        return false;
                } while (it1.MoveNext());
              #else
                do {
                    d2.TryGetValue(it2.Name, out var values);
                    values.Enqueue(it2.Value);
                    d2[it2.Name] = values;
                } while (it2.MoveNext());
                do {
                    if (!d2.TryGetValue(it1.Name, out var values) ||
                        !values.TryDequeue(out var value) ||
                        !JsonElement.DeepEquals(it1.Value, value, options))
                        return false;
                    d2[it1.Name] = values;
                } while (it1.MoveNext());
              #endif
                return true;
            }
          #endif
        }

        private static bool ShallowEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options) =>
            JsonElement.ReferenceEquals(el1, el2, options) ||
            (el1.ValueKind, el2.ValueKind) switch {
                var (k1, k2) => k1 == k2 && JsonElement.ValueEquals(el1, el2, options),
            };

        [SuppressMessage("Style", "IDE0060:Remove unused parameter"), SuppressMessage("ReSharper", "UnusedParameter.Local")]
        public static bool ReferenceEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options) =>
            JsonElement.DocumentOffsetEquals(el1, el2);

        private static bool ValueEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options)
        {
            if (JsonElement.ReferenceEquals(el1, el2, options))
                return true;
          #if JSON9_0_OR_GREATER
            return JsonElement.DeepEquals(el1, el2);
          #else
            var (k1, k2) = (el1.ValueKind, el2.ValueKind);
            return k1 == k2 && k1 switch {
                JsonValueKind.Null or JsonValueKind.Undefined or JsonValueKind.True or JsonValueKind.False =>
                    true,
                JsonValueKind.String =>
                    el1.ValueEquals(el2.GetString()),
                JsonValueKind.Number =>
                    Equals(JsonElement.ToValue(el1, options), JsonElement.ToValue(el2, options)),
                _ =>
                    throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {k1}"),
            };
          #endif
        }

        public static bool DocumentOffsetEquals(in JsonElement el1, in JsonElement el2) =>
            MemoryMarshal.CreateReadOnlyByteSpan(el1).SequenceEqual(MemoryMarshal.CreateReadOnlyByteSpan(el2));

        public static bool IsNull(in JsonElement el, JNodeOptions options) =>
            el.ValueKind switch {
                JsonValueKind.Null => true,
                JsonValueKind.Undefined => options.UndefinedValue == null,
                _ => false,
            };

        [SuppressMessage("Style", "IDE0051", Justification = "C #14 Bug")]
        private static bool IsFloatingPoint(ReadOnlySpan<byte> span) =>
            span.IndexOfAny((byte)'.', (byte)'e', (byte)'E') switch {
                -1 => false,
                var i => span[i] == (byte)'.' || i + 1 < span.Length && span[i + 1] == (byte)'-',
            };
    }

    extension(in JsonProperty @this)
    {
      #if JSON10_0_OR_GREATER
        public ReadOnlySpan<byte> RawNameSpan => JsonMarshal.GetRawUtf8PropertyName(@this);
      #endif
    }

    extension(ref ObjectEnumerator @this)
    {
        public string Name => @this.Current.Name;
        public JsonElement Value => @this.Current.Value;
      #if JSON10_0_OR_GREATER
        public ReadOnlySpan<byte> RawNameSpan => JsonMarshal.GetRawUtf8PropertyName(@this.Current);
      #endif
    }

  #if !JSON9_0_OR_GREATER
    [field: ThreadStatic, MaybeNull]
    private static Utf8JsonElementWriter JsonElementWriter => field ??= new(256);

    private class Utf8JsonElementWriter : IDisposable
    {
        private static readonly JsonWriterOptions JsonWriterOptions = new() {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Indented = false,
            SkipValidation = true,
        };

        private readonly FixedArrayBufferWriter<byte> _buffer;
        private readonly Utf8JsonWriter _writer;

        public Utf8JsonElementWriter(int size)
        {
            _buffer = new(size);
            _writer = new(_buffer, JsonWriterOptions);
        }

        public ReadOnlySpan<byte> WrittenSpan => _buffer.WrittenSpan;

        public void CopyFrom(in JsonElement j)
        {
            _buffer.ResetIndex();
            j.WriteTo(_writer);
        }

        public void Dispose() => _writer.Dispose();
    }
  #endif
}