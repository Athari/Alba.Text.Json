using System.Reflection;

namespace Alba.Text.Json.Dynamic;

internal class MethodKey : IEquatable<MethodKey>
{
    public static readonly Type[] ArgT = Enumerable.Range(0, 1).Select(GetGenericMethodParameter).ToArray();

    public static T GetT<T>(int _) => throw new NotSupportedException();

    public MethodKeyKind Kind;
    public Type Type;
    public string Name;
    public RefEquatableArray<Type> ParameterTypes;
    public RefEquatableArray<Type> GenericTypes;

    private readonly int _hashCode;

    private MethodKey(MethodKeyKind kind, Type type, string name, Type[]? parameterTypes = null, Type[]? genericTypes = null)
    {
        (Kind, Type, Name, ParameterTypes, GenericTypes) =
            (kind, type, name, parameterTypes.AsRefEquatable(), genericTypes.AsRefEquatable());
        _hashCode = CalculateHashCode();
    }

    public static MethodKey Regular(Type type, string name, BindingFlags flags, Type[]? parameterTypes) =>
        new(MethodKeyKind.Regular | flags.ToMethodKeyKind(), type, name, parameterTypes);

    public static MethodKey Regular<T, TArgs>(string name, BindingFlags flags) =>
        Regular(typeof(T), name, flags, typeof(TArgs).GetGenericArguments());

    public static MethodKey RegularInstance<T, TArgs>(string name) where TArgs : Delegate =>
        Regular<T, TArgs>(name, BindingFlags.Instance | BindingFlags.Public);

    public static MethodKey UnboundGeneric(Type type, string name, BindingFlags flags, Type[] parameterTypes) =>
        new(MethodKeyKind.UnboundGeneric | flags.ToMethodKeyKind(), type, name, parameterTypes);

    public static MethodKey Generic(Type type, string name, BindingFlags flags, Type[] parameterTypes, Type[] genericTypes) =>
        new(MethodKeyKind.Generic | flags.ToMethodKeyKind(), type, name, parameterTypes, genericTypes);

    private int CalculateHashCode()
    {
        var hc = new HashCode();
        hc.Add((int)Kind);
        hc.Add(Type.GetHashCode());
        hc.Add(Name);
        hc.Add(ParameterTypes.GetHashCode());
        hc.Add(GenericTypes.GetHashCode());
        return hc.ToHashCode();
    }

    public bool Equals(MethodKey? o) =>
        o != null &&
        Kind == o.Kind &&
        Type == o.Type &&
        string.Equals(Name, o.Name, StringComparison.Ordinal) &&
        ParameterTypes.Equals(o.ParameterTypes) &&
        GenericTypes.Equals(o.GenericTypes);

    public override bool Equals(object? obj) =>
        obj is MethodKey o && Equals(o);

    public override int GetHashCode() =>
        _hashCode;

    public override string ToString() =>
        $"{Type.Name}{Name}{(GenericTypes.Count > 0 ? $"<{ToString(GenericTypes)}>" : "")}({ToString(ParameterTypes)})";

    private static string ToString(RefEquatableArray<Type> types) =>
        string.Join(", ", types.Select(t => t.Name));

    private static Type GetGenericMethodParameter(int i)
    {
      #if NETFRAMEWORK
        //return GenericMethodParameters[i];
        return typeof(object);
      #else
        return Type.MakeGenericMethodParameter(i);
      #endif
    }

  //#if NETFRAMEWORK
  //  private static readonly Type[] GenericMethodParameters = [ typeof(TArg0), typeof(TArg1), typeof(TArg2), typeof(TArg3) ];

  //  private class TArg0;
  //  private class TArg1;
  //  private class TArg2;
  //  private class TArg3;
  //#endif
}