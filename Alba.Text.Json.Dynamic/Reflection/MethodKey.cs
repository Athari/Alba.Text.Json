using System.Reflection;

namespace Alba.Text.Json.Dynamic;

internal class MethodKey : IEquatable<MethodKey>
{
    public static readonly Type[] ArgT = Enumerable.Range(0, 1).Select(Type.MakeGenericMethodParameter).ToArray();

    public static T GetT<T>(int _) => throw new NotSupportedException();

    private readonly MethodKeyKind _kind;
    private readonly Type _type;
    private readonly string _name;
    private readonly Type[]? _parameterTypes;
    private readonly Type[]? _genericTypes;
    private readonly int _hashCode;

    private MethodKey(MethodKeyKind kind, Type type, string name, Type[]? parameterTypes = null, Type[]? genericTypes = null)
    {
        (_kind, _type, _name, _parameterTypes, _genericTypes) = (kind, type, name, parameterTypes, genericTypes);
        _hashCode = CalculateHashCode();
    }

    public static MethodKey Regular(Type type, string name, BindingFlags flags, Type[]? parameterTypes) =>
        new(MethodKeyKind.Regular | flags.ToMethodKeyKind(), type, name, parameterTypes);

    public static MethodKey UnboundGeneric(Type type, string name, BindingFlags flags, Type[] parameterTypes) =>
        new(MethodKeyKind.UnboundGeneric | flags.ToMethodKeyKind(), type, name, parameterTypes);

    public static MethodKey Generic(Type type, string name, BindingFlags flags, Type[] parameterTypes, Type[] genericTypes) =>
        new(MethodKeyKind.Generic | flags.ToMethodKeyKind(), type, name, parameterTypes, genericTypes);

    private int CalculateHashCode()
    {
        var hc = new HashCode();
        hc.Add((int)_kind);
        hc.Add(_type.GetHashCode());
        hc.Add(_name);
        if (_parameterTypes != null)
            foreach (var v in _parameterTypes)
                hc.Add(v.GetHashCode());
        if (_genericTypes != null)
            foreach (var v in _genericTypes)
                hc.Add(v.GetHashCode());
        return hc.ToHashCode();
    }

    public bool Equals(MethodKey? o) =>
        o != null &&
        _kind == o._kind &&
        _type == o._type &&
        string.Equals(_name, o._name, StringComparison.Ordinal) &&
        (_parameterTypes?.SequenceEqual(o._parameterTypes) ?? true) &&
        (_genericTypes?.SequenceEqual(o._genericTypes) ?? true);

    public override bool Equals(object? obj) =>
        obj is MethodKey o && Equals(o);

    public override int GetHashCode() =>
        _hashCode;

    public override string ToString() =>
        $"{_name}{(_genericTypes != null ? $"<{ToString(_genericTypes)}>" : "")}({ToString(_parameterTypes)})";

    private static string? ToString(Type[]? types) =>
        types != null ? string.Join(", ", types.Select(t => t.Name)) : null;
}