using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Alba.Text.Json.Converters;

/// <summary>A <see cref="JsonConverter"/> using <see cref="TypeConverter"/> <typeparamref name="TConverter"/> to serialize a value <typeparamref name="T"/> as representation <typeparamref name="TRepr"/>.</summary>
/// <typeparam name="T">Type of the value.</typeparam>
/// <typeparam name="TRepr">Type of the representation of the value.</typeparam>
/// <typeparam name="TConverter">A <see cref="TypeConverter"/> performing conversion.</typeparam>
public class TypeJsonConverter<T, TRepr, TConverter> : ValueJsonConverter<T>
    where TConverter : TypeConverter, new()
{
    /// <summary>Create a new <see cref="TypeJsonConverter{T,TRepr,TConverter}"/> instance.</summary>
    public TypeJsonConverter() =>
        Converter = new TypeConverterRef<T, TRepr, TConverter>();

    /// <summary>Create a new <see cref="TypeJsonConverter{T,TRepr,TConverter}"/> instance.</summary>
    /// <param name="invert">Set whether to use reverse conversion (<see cref="ValueConverterRef.Invert"/>).</param>
    /// <param name="convertNull">Set whether to pass <see langword="null"/> to the converter (<see cref="ValueConverterRef.ConvertNull"/>).</param>
    /// <param name="culture">Set culture to use for conversion (<see cref="TypeConverterRef{T,TRepr,TConverter}.Culture"/>).</param>
    public TypeJsonConverter(bool invert = false, bool convertNull = false, CultureInfo? culture = null) =>
        Converter = new TypeConverterRef<T, TRepr, TConverter> {
            Invert = invert,
            ConvertNull = convertNull,
            Culture = culture ?? CultureInfo.InvariantCulture,
        };
}