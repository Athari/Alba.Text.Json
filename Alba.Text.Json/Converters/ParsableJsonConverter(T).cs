#if NET7_0_OR_GREATER

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alba.Text.Json.Converters;

/// <summary>A <see cref="JsonConverter"/> using <see cref="IParsable{TSelf}"/> and <see cref="IFormattable"/> implemented by <typeparamref name="T"/> to serialize it as a <see cref="string"/>.</summary>
/// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
public class ParsableJsonConverter<T> : JsonConverter<T>
    where T : IParsable<T>, IFormattable
{
    /// <summary>Set culture to use for conversion. The default is <see cref="CultureInfo.InvariantCulture"/>.</summary>
    public CultureInfo? Culture { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>Sets the format string. Set to <see langword="null"/> to use the default format. The default is <see langword="null"/>.</summary>
    public string? Format { get; set; }

    /// <inheritdoc/>
    public override bool HandleNull => false;

    /// <inheritdoc/>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.String
            ? T.Parse(reader.GetString() ?? "", Culture)
            : throw new FormatException($"String or null expected, got {reader.TokenType}");

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString(Format, Culture));
}

#endif