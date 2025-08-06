#nullable enable

namespace Alba.Text.Json.Dynamic;

public partial class JNodeOptionsAttribute
{
    /// <inheritdoc cref="JNodeOptions.IsCaseSensitive" />
    public bool IsCaseSensitive { get; set; } = JNodeOptions.Default.IsCaseSensitive;

    /// <inheritdoc cref="JNodeOptions.DirectEquality" />
    public Equality DirectEquality { get; set; } = JNodeOptions.Default.DirectEquality;

    /// <inheritdoc cref="JNodeOptions.SearchEquality" />
    public Equality SearchEquality { get; set; } = JNodeOptions.Default.SearchEquality;

    /// <inheritdoc cref="JNodeOptions.MaxHashCodeValueCount" />
    public int MaxHashCodeValueCount { get; set; } = JNodeOptions.Default.MaxHashCodeValueCount;

    /// <inheritdoc cref="JNodeOptions.MaxHashCodeDepth" />
    public int MaxHashCodeDepth { get; set; } = JNodeOptions.Default.MaxHashCodeDepth;

    /// <inheritdoc cref="JNodeOptions.UndefinedValue" />
    public object? UndefinedValue { get; set; } = JNodeOptions.Default.UndefinedValue;

    /// <inheritdoc cref="JNodeOptions.IntegerTypes" />
    public NumberType[] IntegerTypes { get; set; } = JNodeOptions.Default.IntegerTypes.ToArray();

    /// <inheritdoc cref="JNodeOptions.FloatTypes" />
    public NumberType[] FloatTypes { get; set; } = JNodeOptions.Default.FloatTypes.ToArray();

    /// <summary>Convert to a <see cref="JNodeOptions"/> instance by copying all property values.</summary>
    public JNodeOptions ToOptions() => new() {
        IsCaseSensitive = IsCaseSensitive,
        DirectEquality = DirectEquality,
        SearchEquality = SearchEquality,
        MaxHashCodeValueCount = MaxHashCodeValueCount,
        MaxHashCodeDepth = MaxHashCodeDepth,
        UndefinedValue = UndefinedValue,
        IntegerTypes = IntegerTypes.ToArray(),
        FloatTypes = FloatTypes.ToArray(),
    };
}