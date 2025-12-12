namespace Alba.Text.Json.Converters;

/// <summary>A converter between a value and its representation. Should not be implemented directly; derive from <see cref="ValueJsonConverter{T,TRepr}"/> instead.</summary>
public interface IValueJsonConverter
{
    /// <summary>Forward conversion from a value to its representation.</summary>
    /// <param name="o">The value to convert.</param>
    /// <returns>The representation of the value.</returns>
    object? ValueToRepr(object? o);

    /// <summary>Reverse conversion from a representation back to the value.</summary>
    /// <param name="o">The representation of the value.</param>
    /// <returns>The represented value.</returns>
    object? ReprToValue(object? o);
}