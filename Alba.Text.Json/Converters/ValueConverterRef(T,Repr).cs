using System.ComponentModel;

namespace Alba.Text.Json.Converters;

/// <summary>Strongly typed converter between a value <typeparamref name="T"/> and its representation <typeparamref name="TRepr"/>. Derived classes can delegate conversion to <see cref="TypeConverter"/>, <c>IValueConverter</c> of XAML frameworks etc.</summary>
/// <typeparam name="T">Type of the value.</typeparam>
/// <typeparam name="TRepr">Type of the representation of the value.</typeparam>
public abstract class ValueConverterRef<T, TRepr> : ValueConverterRef
{
    /// <summary>Type of the value. Takes <see cref="ValueConverterRef.Invert"/> into account.</summary>
    protected Type ValueType => Invert ? typeof(TRepr) : typeof(T);

    /// <summary>Type of the representation. Takes <see cref="ValueConverterRef.Invert"/> into account.</summary>
    protected Type ReprType => Invert ? typeof(T) : typeof(TRepr);
}