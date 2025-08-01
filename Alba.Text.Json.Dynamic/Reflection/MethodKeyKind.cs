using System.Diagnostics;
using System.Reflection;

namespace Alba.Text.Json.Dynamic;

[Flags]
internal enum MethodKeyKind : uint
{
    Regular = 0x1000_0000,
    UnboundGeneric = 0x2000_0000,
    Generic = 0x4000_0000,
    MaskGeneric = Regular | UnboundGeneric | Generic,

    Public = BindingFlags.Public, // 0x0000_0010
    NonPublic = BindingFlags.NonPublic, // 0x0000_0020
    MaskVisibility = Public | NonPublic, // 0x0000_0030

    Instance = BindingFlags.Instance, // 0x0000_0004
    Static = BindingFlags.Static, // 0x0000_0008
    MaskStatic = Instance | Static, // 0x0000_000C

    CaseSensitive = 0x8000_0000,
    CaseInsensitive = BindingFlags.IgnoreCase,
    MaskCaseSensitivity = CaseSensitive | CaseInsensitive,

    MaskBindingFlags = MaskVisibility | MaskStatic | MaskCaseSensitivity,
}

internal static class MethodKeyKindExts
{
    public static MethodKeyKind ToMethodKeyKind(this BindingFlags @this)
    {
        var kind = (MethodKeyKind)@this;
        Debug.Assert((kind & ~MethodKeyKind.MaskBindingFlags) == 0);
        return kind;
    }
}