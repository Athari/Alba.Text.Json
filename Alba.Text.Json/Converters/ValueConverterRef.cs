using System.ComponentModel;

namespace Alba.Text.Json.Converters;

/// <summary>Converter between a value and its representation. Derived classes can delegate conversion to <see cref="TypeConverter"/>, <c>IValueConverter</c> of XAML frameworks etc.</summary>
public abstract class ValueConverterRef
{
    /// <summary>Sets whether to perform forward or reverse conversion.</summary>
    public bool Invert { get; set; }

    /// <summary>Sets whether to pass <see langword="null"/> to the converter or forward it as-is.</summary>
    public bool ConvertNull { get; set; }

    /// <summary>Forward conversion from a value to its representation. Takes <see cref="Invert"/> and <see cref="ConvertNull"/> into account.</summary>
    /// <param name="o">The value to convert.</param>
    /// <returns>The representation of the value.</returns>
    public object? ValueToRepr(object? o) => o != null || ConvertNull ? ValueToReprCore(o) : null;

    /// <summary>Reverse conversion from a representation back to the value. Takes <see cref="Invert"/> and <see cref="ConvertNull"/> into account.</summary>
    /// <param name="o">The representation of the value.</param>
    /// <returns>The represented value.</returns>
    public object? ReprToValue(object? o) => o != null || ConvertNull ? ReprToValueCore(o) : null;

    private object? ValueToReprCore(object? o) => Invert ? ReprToValueOverride(o) : ValueToReprOverride(o);
    private object? ReprToValueCore(object? o) => Invert ? ValueToReprOverride(o) : ReprToValueOverride(o);

    /// <summary>Forward conversion from a value to its representation. Implement in a derived class using <see cref="TypeConverter"/>, <c>IValueConverter</c> of XAML frameworks etc.</summary>
    /// <param name="o">The value to convert.</param>
    /// <returns>The representation of the value.</returns>
    protected abstract object? ValueToReprOverride(object? o);

    /// <summary>Reverse conversion from a representation back to the value. Implement in a derived class using <see cref="TypeConverter"/>, <c>IValueConverter</c> of XAML frameworks etc.</summary>
    /// <param name="o">The representation of the value.</param>
    /// <returns>The represented value.</returns>
    protected abstract object? ReprToValueOverride(object? o);
}