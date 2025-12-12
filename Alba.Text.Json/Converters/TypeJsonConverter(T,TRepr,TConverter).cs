using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Alba.Text.Json.Converters;

/// <summary>A <see cref="JsonConverter"/> using <see cref="TypeConverter"/> <typeparamref name="TConverter"/> to serialize a value <typeparamref name="T"/> as representation <typeparamref name="TRepr"/>.</summary>
/// <typeparam name="T">Type of the value.</typeparam>
/// <typeparam name="TRepr">Type of the representation of the value.</typeparam>
/// <typeparam name="TConverter">A <see cref="TypeConverter"/> performing conversion.</typeparam>
public class TypeJsonConverter<T, TRepr, TConverter> : ValueJsonConverter<T, TRepr>
    where TConverter : TypeConverter, new()
{
    /// <summary>Create a new <see cref="TypeJsonConverter{T,TRepr,TConverter}"/> instance.</summary>
    /// <param name="invert">Sets whether to perform forward or reverse conversion.</param>
    /// <param name="canHandleNull">Sets whether to pass <see langword="null"/> to the converter or forward it as-is.</param>
    /// <param name="culture">Set culture to use for conversion (<see cref="Culture"/>).</param>
    public TypeJsonConverter(bool invert = false, bool canHandleNull = false, CultureInfo? culture = null)
        : base(invert, canHandleNull) =>
        Culture = culture ?? CultureInfo.InvariantCulture;

    /// <summary>A <see cref="CultureInfo"/> used for conversion. <see cref="CultureInfo.InvariantCulture"/> is used by default.</summary>
    public CultureInfo Culture { get; set; }

    /// <summary>An instance of <typeparamref name="TConverter"/>.</summary>
    public TConverter Converter => field ??= new();

    /// <inheritdoc/>
    protected override object? ValueToReprOverride(object? o) =>
        Converter.ConvertTo(null, Culture, o, ReprType);

    /// <inheritdoc/>
    protected override object? ReprToValueOverride(object? o) =>
        Converter.ConvertFrom(null, Culture, o!);
}