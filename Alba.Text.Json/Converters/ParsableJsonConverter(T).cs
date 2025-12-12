using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alba.Text.Json.Converters;

public class ParsableJsonConverter<T> : JsonConverter<T?>
    where T : IParsable<T>, IFormattable
{
    private const int MaxStackAllocSize = 256;

    public CultureInfo? Culture { get; set; } = CultureInfo.InvariantCulture;
    public string? Format { get; set; }

    /// <inheritdoc/>
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch {
            JsonTokenType.Null => default,
            JsonTokenType.String => T.Parse(reader.GetString() ?? "", Culture),
            _ => throw new FormatException($"String or null expected, got {reader.TokenType}"),
        };

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.ToString(Format, Culture));
    }
}