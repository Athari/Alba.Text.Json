using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Alba.Text.Json.Extensions;

namespace Alba.Text.Json.Converters;

/// <summary>A <see cref="JsonConverter{T}"/> using <see cref="ValueTypeExts.ToJsonElementOrNode{T}"/> and <see cref="JsonElementExts.ToValue"/> to read/write a JSON value. The conversion between a value <typeparamref name="T"/> and its representation <typeparamref name="TRepr"/> is abstract. Derived classes can delegate conversion to <see cref="TypeConverter"/>, <c>IValueConverter</c> of XAML frameworks etc.</summary>
/// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
/// <typeparam name="TRepr">Type of the representation of the value.</typeparam>
public abstract class ValueJsonConverter<T, TRepr> : JsonConverter<T?>, IValueJsonConverter
{
    /// <summary>Sets whether to perform forward or reverse conversion.</summary>
    public bool Invert { get; set; }

    /// <summary>Sets whether to pass <see langword="null"/> to the converter or forward it as-is.</summary>
    public bool CanHandleNull { get; set; }

    /// <summary>Creates a new <see cref="ValueJsonConverter{T,TRepr}"/> instance.</summary>
    /// <param name="invert">Sets whether to perform forward or reverse conversion.</param>
    /// <param name="canHandleNull">Sets whether to pass <see langword="null"/> to the converter or forward it as-is.</param>
    protected ValueJsonConverter(bool invert = false, bool canHandleNull = false) =>
        (Invert, CanHandleNull) = (invert, canHandleNull);

    /// <inheritdoc/>
    public override bool HandleNull => CanHandleNull;

    /// <summary>Type of the value. Takes <see cref="Invert"/> into account.</summary>
    protected Type ValueType => Invert ? typeof(TRepr) : typeof(T);

    /// <summary>Type of the representation. Takes <see cref="Invert"/> into account.</summary>
    protected Type ReprType => Invert ? typeof(T) : typeof(TRepr);

    /// <summary>Forward conversion from a value to its representation. Takes <see cref="Invert"/> and <see cref="CanHandleNull"/> into account.</summary>
    /// <param name="o">The value to convert.</param>
    /// <returns>The representation of the value.</returns>
    public object? ValueToRepr(object? o) => o != null || CanHandleNull ? ValueToReprCore(o) : null;

    /// <summary>Reverse conversion from a representation back to the value. Takes <see cref="Invert"/> and <see cref="CanHandleNull"/> into account.</summary>
    /// <param name="o">The representation of the value.</param>
    /// <returns>The represented value.</returns>
    public object? ReprToValue(object? o) => o != null || CanHandleNull ? ReprToValueCore(o) : null;

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

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        var r = ValueToRepr(value);
        writer.WriteValue(r);
    }

    /// <inheritdoc/>
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var v = ReprToValue(reader.GetValue(JNodeOptions.Default));
        return v is T t ? t : default;
    }
}