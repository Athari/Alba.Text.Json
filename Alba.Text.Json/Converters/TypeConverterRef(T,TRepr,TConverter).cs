using System.ComponentModel;
using System.Globalization;

namespace Alba.Text.Json.Converters;

/// <summary>A <see cref="ValueConverterRef{T,TRepr}"/> delegating conversion to <see cref="TypeConverter"/> <typeparamref name="TConverter"/> to convert between a value <typeparamref name="T"/> and its representation <typeparamref name="TRepr"/>.</summary>
/// <typeparam name="T">Type of the value.</typeparam>
/// <typeparam name="TRepr">Type of the representation of the value.</typeparam>
/// <typeparam name="TConverter">A <see cref="TypeConverter"/> performing conversion.</typeparam>
public class TypeConverterRef<T, TRepr, TConverter> : ValueConverterRef<T, TRepr>
    where TConverter : TypeConverter, new()
{
    private static readonly TConverter _DefaultConverter = new();

    /// <summary>A <see cref="CultureInfo"/> used for conversion. <see cref="CultureInfo.InvariantCulture"/> is used by default.</summary>
    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>Creates a new instance of <see cref="TypeConverterRef{T,TRepr,TConverter}"/>. Sets <see cref="ValueConverterRef.ConvertNull"/> to <see langword="false"/> by default.</summary>
    public TypeConverterRef() => ConvertNull = false;

    /// <summary>An instance of <typeparamref name="TConverter"/>.</summary>
    public TConverter Converter => _DefaultConverter;

    /// <inheritdoc/>
    protected override object? ValueToReprOverride(object? o) =>
        Converter.ConvertTo(null, Culture, o, ReprType);

    /// <inheritdoc/>
    protected override object? ReprToValueOverride(object? o) =>
        Converter.ConvertFrom(null, Culture, o!);
}