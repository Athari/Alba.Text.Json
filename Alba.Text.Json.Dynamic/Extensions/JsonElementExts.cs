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
        /// <inheritdoc cref="JsonElement.GetRawText" />
        public string RawText => @this.GetRawText();

      #if JSON9_0_OR_GREATER
        /// <inheritdoc cref="JsonMarshal.GetRawUtf8Value" />
      #endif
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

        public object? ToValue(JNodeOptions options) =>
            @this.ValueKind switch {
                JsonValueKind.Undefined => options.UndefinedValue,
                JsonValueKind.Null => null,
                JsonValueKind.String => @this.GetString(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Number => @this.ToNumber(options),
                JsonValueKind.Object or JsonValueKind.Array =>
                    throw new InvalidOperationException($"{@this.ValueKind} JsonElement in JsonValue"),
                _ => throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {@this.ValueKind}"),
            };

        public object ToNumber(JNodeOptions options) =>
            IsFloatingPoint(@this.RawValueSpan) ?
                @this.ToNumberType(options.FloatTypes) ??
                throw new InvalidOperationException($"Cannot convert {@this} to a floating point number.") :
                @this.ToNumberType(options.IntegerTypes) ??
                throw new InvalidOperationException($"Cannot convert {@this} to an integer number.");

        private object? ToNumberType(NumberType[] types)
        {
            var el = @this;
            return types.Select(t => el.ToNumberType(t)).FirstOrDefault(v => v != null);
        }

        [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", Justification = "Intentional")]
        private object? ToNumberType(NumberType type) =>
            type switch {
                NumberType.SByte => @this.TryGetSByte(out var v) ? v : null,
                NumberType.Byte => @this.TryGetByte(out var v) ? v : null,
                NumberType.Int16 => @this.TryGetInt16(out var v) ? v : null,
                NumberType.UInt16 => @this.TryGetUInt16(out var v) ? v : null,
                NumberType.Int32 => @this.TryGetInt32(out var v) ? v : null,
                NumberType.UInt32 => @this.TryGetUInt32(out var v) ? v : null,
                NumberType.Int64 => @this.TryGetInt64(out var v) ? v : null,
                NumberType.UInt64 => @this.TryGetUInt64(out var v) ? v : null,
                NumberType.Single => @this.TryGetSingle(out var v) ? v : null,
                NumberType.Double => @this.TryGetDouble(out var v) ? v : null,
                NumberType.Decimal => @this.TryGetDecimal(out var v) ? v : null,
              #if NET8_0_OR_GREATER
                NumberType.Half => Half.TryParse(@this.RawValueSpan, Invariant, out var v) ? v : null,
              #elif NET5_0_OR_GREATER
                NumberType.Half => Half.TryParse(@this.RawText, NumberStyles.Float | NumberStyles.AllowThousands, Invariant, out var v) ? v : null,
              #endif
              #if NET8_0_OR_GREATER
                NumberType.Int128 => Int128.TryParse(@this.RawValueSpan, Invariant, out var v) ? v : null,
                NumberType.UInt128 => UInt128.TryParse(@this.RawValueSpan, Invariant, out var v) ? v : null,
              #elif NET7_0_OR_GREATER
                NumberType.Int128 => Int128.TryParse(@this.RawText, Invariant, out var v) ? v : null,
                NumberType.UInt128 => UInt128.TryParse(@this.RawText, Invariant, out var v) ? v : null,
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
        private static bool ReferenceEquals(in JsonElement el1, in JsonElement el2, JNodeOptions options) =>
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
                    Equals(el1.ToValue(options), el2.ToValue(options)),
                _ =>
                    throw new InvalidOperationException($"Unexpected JsonValueKind of JsonElement: {k1}"),
            };
          #endif
        }

        public static bool DocumentOffsetEquals(in JsonElement el1, in JsonElement el2) =>
            MemoryMarshal.CreateReadOnlyByteSpan(el1).SequenceEqual(MemoryMarshal.CreateReadOnlyByteSpan(el2));

        public bool IsNull(JNodeOptions options) =>
            @this.ValueKind switch {
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