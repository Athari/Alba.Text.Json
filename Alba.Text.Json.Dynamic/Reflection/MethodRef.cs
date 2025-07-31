using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Alba.Text.Json.Dynamic;

internal class MethodRef
{
    public readonly Type Type;
    public readonly string Name;
    public readonly Type[] ParameterTypes;
    public readonly BindingFlags Flags;

    public readonly MethodInfo? Method;
    public readonly MethodInfo? UnboundGenericMethod;

    public readonly MethodKey? Key;
    public readonly MethodKey? UnboundGenericKey;

    private MethodRef(LambdaExpression expr)
    {
        var call = (MethodCallExpression)expr.Body;
        var method = call.Method;
        int genericCount = 0;

        Type = method.DeclaringType!;
        Name = method.Name;
        ParameterTypes = method.GetParameters()
            .Select(p => p.ParameterType)
            .Zip(call.Arguments, (t, a) => TryGetGenericArgIndex(a, ref genericCount) ?? t)
            .ToArray();
        Flags =
            (method.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic)
          | (method.IsStatic ? BindingFlags.Static : BindingFlags.Instance);
        (Method, Key, UnboundGenericMethod, UnboundGenericKey) = genericCount switch {
            0 => (
                method,
                MethodKey.Regular(Type, Name, Flags, ParameterTypes),
                (MethodInfo?)null,
                (MethodKey?)null),
            _ => (
                null,
                null,
                Type.GetMethod(Name, genericCount, Flags, ParameterTypes),
                MethodKey.UnboundGeneric(Type, Name, Flags, ParameterTypes)),
        };
    }

    public static MethodRef Of<TResult>(Expression<Func<TResult>> expr) => new(expr); // static method
    public static MethodRef Of<T, TResult>(Expression<Func<T, TResult>> expr) => new(expr); // instance method

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MethodKey GenericKey(Type[] genericTypes) =>
        MethodKey.Generic(Type, Name, Flags, ParameterTypes, genericTypes);

    private static Type? TryGetGenericArgIndex(E expr, ref int genericCount)
    {
        if (expr is not MethodCallExpression call)
            return null;
        genericCount++;
        var val = (ConstantExpression)call.Arguments.Single();
        return MethodKey.ArgT[(int)val.Value!];
    }
}