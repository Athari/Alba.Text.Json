using System.Text.Json;
#if JSON9_0_OR_GREATER
using System.Runtime.InteropServices;
#else
using System.Text.Encodings.Web;
#endif

namespace Alba.Text.Json.Dynamic;

internal static class JsonElementExts
{
    public static ReadOnlySpan<byte> GetRawValueSpan(this JsonElement @this)
    {
      #if JSON9_0_OR_GREATER
        return JsonMarshal.GetRawUtf8Value(@this);
      #else
        var writer = JsonElementWriter;
        writer.CopyFrom(@this);
        return writer.WrittenSpan;
      #endif
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

        public void CopyFrom(JsonElement j)
        {
            _buffer.ResetIndex();
            j.WriteTo(_writer);
        }
    }
  #endif
}