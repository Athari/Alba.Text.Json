#nullable enable

namespace Alba.Text.Json.Dynamic;

public partial class JNodeOptionsAttribute
{
    public bool IsCaseSensitive { get; set; } = true;

    public int MaxHashCodeValueCount { get; set; } = 32;

    public int MaxHashCodeDepth { get; set; } = 4;

    public object? UndefinedValue { get; set; } = null;

    public TypeCode[] IntegerTypes { get; set; } = [ TypeCode.Int32, TypeCode.Int64, TypeCode.UInt64, TypeCode.Decimal ];

    public TypeCode[] FloatTypes { get; set; } = [ TypeCode.Single, TypeCode.Double, TypeCode.Decimal ];
}