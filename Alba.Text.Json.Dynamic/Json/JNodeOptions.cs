using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

#pragma warning disable CS1633 // Unrecognized #pragma directive

/// <summary>Options for controlling behavior of <see cref="JNode"/>.</summary>
public sealed class JNodeOptions
{
    /// <summary>Default options set.</summary>
    public static readonly JNodeOptions Default = new();

    internal JsonNodeOptions JsonNodeOptions => new() { PropertyNameCaseInsensitive = !IsCaseSensitive };

    internal StringComparison PropertyNameComparison => IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

    internal StringComparer PropertyNameComparer => IsCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;

    internal bool IsNull(object? o) => o == null || Equals(UndefinedValue, null) && IsUndefined(o);

    internal bool IsUndefined(object? o) => Equals(UndefinedValue, o);

    internal bool ArePropertyNamesEqual(string a, string b) => a.Equals(b, PropertyNameComparison);

#pragma t4 copy begin
    /// <summary>
    /// Gets or sets a value that indicates whether property names on <see cref="JObject"/> are case-sensitive.
    /// </summary>
    public bool IsCaseSensitive { get; set; } = true;

    /// <summary>Equality mode of <see cref="JNode.Equals(object?)"/>,
    /// <see cref="JNode.GetHashCode"/> and other direct equality operations. Default: <see cref="Equality.Deep"/>.</summary>
    public Equality DirectEquality { get; set; } = Equality.Deep;

    /// <summary>Equality mode of <see cref="JArray.IndexOf{T}"/>, <see cref="JArray.Contains{T}"/>,
    /// <see cref="JArray.Remove{T}"/> and other search operations. Default: <see cref="Equality.Shallow"/>.</summary>
    public Equality SearchEquality { get; set; } = Equality.Shallow;

    /// <summary>Maximum number of values to include in the calculation of <see cref="JNode.GetHashCode"/>
    /// when in <see cref="Equality.Deep"/> mode.</summary>
    public int MaxHashCodeValueCount { get; set; } = 32;

    /// <summary>Maxmium depth of arrays and objects to include in the calculation of <see cref="JNode.GetHashCode"/>
    /// when in <see cref="Equality.Deep"/> mode.</summary>
    public int MaxHashCodeDepth { get; set; } = 4;

    /// <summary>Value of nodes of <see cref="JsonValueKind.Undefined"/> kind.</summary>
    public object? UndefinedValue { get; set; } = null;

    /// <summary>The order of numeric types to try to convert to from integer JSON numbers.
    /// Default: [ <see cref="NumberType.Int32"/>, <see cref="NumberType.Int64"/>, <see cref="NumberType.UInt64"/>, <see cref="NumberType.Decimal"/> ].</summary>
    public NumberType[] IntegerTypes { get; set; } = [ NumberType.Int32, NumberType.Int64, NumberType.UInt64, NumberType.Decimal ];

    /// <summary>The order of numeric types to try to convert to from floating point JSON numbers.
    /// Default: [ <see cref="NumberType.Double"/>, <see cref="NumberType.Decimal"/> ].</summary>
    public NumberType[] FloatTypes { get; set; } = [ NumberType.Double, NumberType.Decimal ];
#pragma t4 copy end
}