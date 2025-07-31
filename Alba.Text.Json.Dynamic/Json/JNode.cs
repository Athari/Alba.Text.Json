using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Reflection;
using System.Text.Json.Nodes;
using CommunityToolkit.Diagnostics;
using JetBrains.Annotations;

namespace Alba.Text.Json.Dynamic;

public abstract partial class JNode(JNodeOptions? options = null)
{
    internal readonly JNodeOptions Options = options ?? JNodeOptions.Default;

    internal abstract JsonNode NodeUntyped { get; }

    private protected abstract class MetaJsonNodeBase(E expression, JNode value)
        : dobject(expression, BindingRestrictions.Empty, value)
    {
        [field: ThreadStatic, MaybeNull]
        private static Dictionary<MethodKey, MethodInfo> MethodsCache => field ??= [ ];

        protected BindingRestrictions GetRestrictions() =>
            BindingRestrictions.GetTypeRestriction(Expression, LimitType);

        protected dobject CallStaticMethod(MethodRef m, E[] parameters, Type[]? genericTypes = null) =>
            new(E.Call(null, GetMethod(m, genericTypes), parameters), GetRestrictions());

        protected dobject CallMethod(MethodRef m, E instance, E[] parameters, Type[]? genericTypes = null) =>
            new(E.Call(instance, GetMethod(m, genericTypes), parameters), GetRestrictions());

        private static MethodInfo GetMethod(MethodRef m, Type[]? genericTypes = null)
        {
            if (m.Method != null)
                return m.Method;
            else if (m.UnboundGenericMethod != null) {
                Guard.IsNotNull(genericTypes);
                return CacheGetOrAdd(MethodsCache, m.GenericKey(genericTypes),
                    () => m.UnboundGenericMethod.MakeGenericMethod(genericTypes));
            }
            else
                throw new InvalidOperationException();
        }

        [return: NotNullIfNotNull(nameof(getMethod))]
        private static MethodInfo? CacheGetOrAdd(Dictionary<MethodKey, MethodInfo> cache,
            in MethodKey key, [InstantHandle] Func<MethodInfo?>? getMethod)
        {
            if (cache.TryGetValue(key, out var method))
                return method;
            if (getMethod == null)
                return null;
            WriteLine($"Resolving {key}");
            method = getMethod() ?? throw new InvalidOperationException($"Method {key} not found.");
            cache.Add(key, method);
            return method;
        }
    }
}