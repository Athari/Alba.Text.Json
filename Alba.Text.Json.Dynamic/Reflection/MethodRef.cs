using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Alba.Text.Json.Dynamic;

internal sealed class MethodRef
{
    [field: ThreadStatic, MaybeNull]
    private static Dictionary<MethodKey, MethodInfo> MethodsCache => field ??= [ ];

    public readonly Type Type;
    public readonly string Name;
    public readonly Type[] ParameterTypes;
    public readonly BindingFlags Flags;

    public readonly MethodInfo? Method;
    public readonly MethodInfo? UnboundGenericMethod;

    public readonly MethodKey? Key;
    public readonly MethodKey? UnboundGenericKey;

    public readonly bool IsVoid;

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

        if (genericCount == 0) {
            Method = method;
            Key = MethodKey.Regular(Type, Name, Flags, ParameterTypes);
            (UnboundGenericMethod, UnboundGenericKey) = (null, null);
        }
        else {
            UnboundGenericMethod = method.GetGenericMethodDefinition();
            UnboundGenericKey = MethodKey.UnboundGeneric(Type, Name, Flags, ParameterTypes);
            (Method, Key) = (null, null);
        }

        IsVoid = (Method ?? UnboundGenericMethod)!.ReturnType == typeof(void);
    }

    private MethodRef(MethodKey key)
    {
        Ensure.NotNull(key.ParameterTypes);
        Type = key.Type;
        Name = key.Name;
        ParameterTypes = key.ParameterTypes.ArrayOrEmpty();
        Flags = key.Kind.ToBindingFlags();

        (Method, Key, UnboundGenericMethod, UnboundGenericKey) = key.GenericTypes.Count switch {
            0 => (ResolveMethod(), key, (MethodInfo?)null, (MethodKey?)null),
            _ => (null, null, ResolveMethod(key.GenericTypes.ArrayOrEmpty()), key),
        };

        IsVoid = (Method ?? UnboundGenericMethod)!.ReturnType == typeof(void);
    }

    public static MethodRef Of(Expression<Action> expr) => new(expr);
    public static MethodRef Of<T>(Expression<Action<T>> expr) => new(expr);
    public static MethodRef Of<TResult>(Expression<Func<TResult>> expr) => new(expr);
    public static MethodRef Of<T, TResult>(Expression<Func<T, TResult>> expr) => new(expr);
    public static MethodRef Of(MethodKey key) => new(key);

    private MethodInfo ResolveMethod(Type[]? genericTypes = null)
    {
        return genericTypes == null
            ? Type.GetMethod(Name, Flags, ParameterTypes) ?? throw NoMethod()
            : Type.GetMethod(Name, genericTypes.Length, Flags, ParameterTypes) ?? throw NoMethod();

        InvalidOperationException NoMethod() => new($"Could not resolve method {Name}.");
    }

    public MethodInfo GetMethod(Type[]? genericTypes = null)
    {
        if (Method != null)
            return Method;
        else if (UnboundGenericMethod != null)
            return CacheGetOrAdd(MethodsCache, GenericKey(Ensure.NotNull(genericTypes)),
                () => UnboundGenericMethod.MakeGenericMethod(genericTypes));
        else
            throw new InvalidOperationException();
    }

    [return: NotNullIfNotNull(nameof(getMethod))]
    private static MethodInfo? CacheGetOrAdd(Dictionary<MethodKey, MethodInfo> cache,
        MethodKey key, [InstantHandle] Func<MethodInfo?>? getMethod)
    {
        if (cache.TryGetValue(key, out var method))
            return method;
        if (getMethod == null)
            return null;
        Trace.WriteLine($"Resolving {key}");
        method = getMethod() ?? throw new InvalidOperationException($"Method {key} not found.");
        cache.Add(key, method);
        return method;
    }

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