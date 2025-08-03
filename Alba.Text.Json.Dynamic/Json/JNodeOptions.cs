namespace Alba.Text.Json.Dynamic;

#pragma warning disable CS1633 // Unrecognized #pragma directive

public sealed class JNodeOptions
{
    internal static readonly JNodeOptions Default = new();

#pragma t4 copy begin
    public bool IsCaseSensitive { get; set; } = true;

    public int MaxHashCodeValueCount { get; set; } = 32;

    public int MaxHashCodeDepth { get; set; } = 4;

    public object? UndefinedValue { get; set; } = null;

    public TypeCode[] IntegerTypes { get; set; } = [ TypeCode.Int32, TypeCode.Int64, TypeCode.UInt64, TypeCode.Decimal ];

    public TypeCode[] FloatTypes { get; set; } = [ TypeCode.Single, TypeCode.Double, TypeCode.Decimal ];
#pragma t4 copy end

    public JNodeOptions() { }

    internal JNodeOptions(JNodeOptionsAttribute o)
    {
        IsCaseSensitive = o.IsCaseSensitive;
        MaxHashCodeValueCount = o.MaxHashCodeValueCount;
        MaxHashCodeDepth = o.MaxHashCodeDepth;
        UndefinedValue = o.UndefinedValue;
        IntegerTypes = o.IntegerTypes;
        FloatTypes = o.FloatTypes;
    }
}