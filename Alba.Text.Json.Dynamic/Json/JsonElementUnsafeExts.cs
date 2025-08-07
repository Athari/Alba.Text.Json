using System.Runtime.CompilerServices;
using System.Text.Json;
#if JSON9_0_OR_GREATER
using System.Runtime.InteropServices;
#elif NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
#else
using System.Text.Encodings.Web;
#endif

namespace Alba.Text.Json.Dynamic;

internal static class JsonElementUnsafeExts
{
    extension(in JsonElement @this)
    {
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

        public static bool DocumentOffsetEquals(in JsonElement el1, in JsonElement el2)
        {
          #if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            var span1 = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in el1), 1));
            var span2 = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in el2), 1));
            return span1.SequenceEqual(span2);
          #else
            var el1m = Unsafe.As<JsonElement, JsonElementMorozov>(ref Unsafe.AsRef(in el1));
            var el2m = Unsafe.As<JsonElement, JsonElementMorozov>(ref Unsafe.AsRef(in el2));
            return el1m._parent == el2m._parent && el1m._idx == el2m._idx;
          #endif
        }
    }

  #if !JSON9_0_OR_GREATER
    [field: ThreadStatic, MaybeNull]
    private static Utf8JsonElementWriter JsonElementWriter => field ??= new(256);

    private class Utf8JsonElementWriter
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
    }
  #endif

  #if !NETCOREAPP2_1_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
    private readonly struct JsonElementMorozov()
    {
        public readonly JsonDocument _parent = null!;
        public readonly int _idx = 0;
    }
  #endif
}