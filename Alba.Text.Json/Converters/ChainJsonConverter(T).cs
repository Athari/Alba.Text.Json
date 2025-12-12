using System.Text.Json;
using System.Text.Json.Serialization;
using Alba.Text.Json.Extensions;

namespace Alba.Text.Json.Converters;

/// <summary>A <see cref="JsonConverter"/> chaining multiple <see cref="ValueConverterRef"/> to serialize a value <typeparamref name="T"/>.</summary>
/// <typeparam name="T">Type of the value.</typeparam>
public class ChainJsonConverter<T> : JsonConverter<T>
{
    /// <summary>List of converters to call in sequence.</summary>
    public IList<ValueConverterRef> Converters { get; } = [ ];

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var r = Converters.Aggregate((object?)value, (v, conv) => conv.ValueToRepr(v));
        writer.WriteValue(r);
    }

    /// <inheritdoc/>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var v = Converters.Reverse().Aggregate(reader.GetValue(JNodeOptions.Default), (r, conv) => conv.ReprToValue(r));
        return v is T t ? t : default!;
    }
}