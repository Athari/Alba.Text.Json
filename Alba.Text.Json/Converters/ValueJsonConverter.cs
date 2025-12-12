using System.Text.Json;
using System.Text.Json.Serialization;
using Alba.Text.Json.Extensions;

namespace Alba.Text.Json.Converters;

/// <summary>A <see cref="JsonConverter{T}"/> using <see cref="ValueTypeExts.ToJsonElementOrNode{T}"/> and <see cref="JsonElementExts.ToValue"/> to read/write a JSON value and delegating conversion to <see cref="Converter"/>.</summary>
/// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
public class ValueJsonConverter<T> : JsonConverter<T?>
{
    /// <summary>Converter between a value and its representation.</summary>
    public ValueConverterRef? Converter { get; set; }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        var r = Converter?.ValueToRepr(value);
        writer.WriteValue(r);
    }

    /// <inheritdoc/>
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var v = Converter?.ReprToValue(reader.GetValue(JNodeOptions.Default));
        return v is T t ? t : default;
    }
}