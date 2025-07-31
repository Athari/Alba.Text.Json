#nullable enable

namespace Alba.Text.Json.Dynamic;

public partial class JNodeOptionsAttribute
{
    public bool IsCaseSensitive { get; set; } = true;

    public TypeCode[] IntegerTypes { get; set; } = [ TypeCode.Int32, TypeCode.Int64, TypeCode.UInt64, TypeCode.Decimal ];

    public TypeCode[] FloatTypes { get; set; } = [ TypeCode.Single, TypeCode.Double, TypeCode.Decimal ];
}