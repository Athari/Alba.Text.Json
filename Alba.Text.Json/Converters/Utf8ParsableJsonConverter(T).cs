#if NET8_0_OR_GREATER

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alba.Text.Json.Converters;

public class Utf8ParsableJsonConverter<T> : JsonConverter<T?>
    where T : IUtf8SpanParsable<T>, IUtf8SpanFormattable, IParsable<T>, IFormattable
{
    private const int MaxStackAllocSize = 256;

    public CultureInfo? Culture { get; set; } = CultureInfo.InvariantCulture;
    public string? Format { get; set; }

    /// <inheritdoc/>
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return default;

        if (reader.TokenType != JsonTokenType.String)
            throw new FormatException($"String or null expected, got {reader.TokenType}");

        var length = reader.HasValueSequence ? (int)reader.ValueSequence.Length : reader.ValueSpan.Length;
        if (length > MaxStackAllocSize)
            return T.Parse(reader.GetString() ?? "", Culture);

        if (!reader.ValueIsEscaped && !reader.HasValueSequence)
            return T.Parse(reader.ValueSpan, Culture);

        Span<byte> bytes = stackalloc byte[length];
        var unescapedLength = reader.CopyString(bytes);
        return T.Parse(bytes[..unescapedLength], Culture);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value == null) {
            writer.WriteNullValue();
            return;
        }

        Span<byte> bytes = stackalloc byte[MaxStackAllocSize];
        if (value.TryFormat(bytes, out var length, Format ?? ReadOnlySpan<char>.Empty, Culture))
            writer.WriteStringValue(bytes[..length]);
        else
            writer.WriteStringValue(value.ToString(Format, Culture));
    }
}

#endif