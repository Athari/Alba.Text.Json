using System.Dynamic;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Alba.Text.Json.Extensions;

namespace Alba.Text.Json.Dynamic;

/// <summary>Non-generic base of dynamic adapters for <see cref="JsonNode"/> types with support for dynamic dispatch via <see cref="IDynamicMetaObjectProvider"/>.</summary>
/// <param name="options">Options to control the behavior.</param>
[JsonConverter(typeof(JNodeConverter))]
public abstract partial class JNode(JNodeOptions? options = null) : IJNode, IEquatable<object?>
{
    private static readonly MethodRef PClone = MethodRef.Of((JNode o) => o.Clone());
    private static readonly MethodRef PEquals = MethodRef.Of((JNode o) => o.Equals(0));
    private static readonly MethodRef PGetHashCode = MethodRef.Of((JNode o) => o.GetHashCode());

    private static readonly PropertyRef PNodeUntyped = PropertyRef.Of((JNode o) => o.NodeUntyped);
    private static readonly MethodRef PNodeToJsonString = MethodRef.Of((JsonNode o) => o.ToJsonString(null));
    private static readonly MethodRef PNodeGetPath = MethodRef.Of((JsonNode o) => o.GetPath());

    internal readonly JNodeOptions Options = options ?? JNodeOptions.Default;

    internal abstract JsonNode NodeUntyped { get; }

    JsonNode IJNode.Node => NodeUntyped;

    /// <summary>Gets the JSON path.</summary>
    /// <returns>The JSON Path value.</returns>
    public string GetPath() =>
        NodeUntyped.GetPath();

    /// <summary>Creates a new instance of the <see cref="JNode"/>. All children nodes are recursively cloned.</summary>
    /// <returns>A new cloned instance of the current node.</returns>
    public object Clone() =>
        NodeUntyped.DeepClone().ToJNodeOrValue(Options);

    /// <summary>Determines whether the current <see cref="JNode"/> and the specified <see cref="object"/> are considered equal. It uses <see cref="JNodeOptions.DirectEquality"/> as comparison kind.</summary>
    /// <param name="o">The <see cref="object"/> to compare. The object can be <see cref="JNode"/>, <see cref="JsonNode"/>, <see cref="JsonElement"/>, <see cref="JsonDocument"/>, or anything that can be serialized to <see cref="JsonNode"/>.</param>
    /// <returns><see langword="true"/> if the node and the object are considered equal; otherwise, <see langword="false"/>.</returns>
    public sealed override bool Equals(object? o) =>
        JsonNode.Equals(NodeUntyped, o, Options.DirectEquality, Options);

    /// <summary>Hash code function which correspeconds to <see cref="JNodeOptions.DirectEquality"/> comparison kind.</summary>
    /// <returns>A hash code of the node.</returns>
    public sealed override int GetHashCode() =>
        NodeUntyped.GetHashCode(Options.DirectEquality, Options);

    /// <summary>Converts the current instance to string in JSON format.</summary>
    /// <param name="options">Options to control the serialization behavior.</param>
    /// <returns>JSON representation of current instance.</returns>
    public string ToJsonString(JsonSerializerOptions? options) =>
        NodeUntyped.ToJsonString(options);

    /// <summary>Determines whether the specified node and object are considered equal. It uses <see cref="JNodeOptions.DirectEquality"/> of the first non-null <see cref="JNode"/> as comparison kind.</summary>
    public static bool operator ==(JNode? a, object? b)
    {
        var options = a?.Options ?? (b as JNode)?.Options ?? JNodeOptions.Default;
        return JsonNode.Equals(a?.NodeUntyped, b, options.DirectEquality, options);
    }

    /// <summary>Determines whether the specified object and node are considered equal. It uses <see cref="JNodeOptions.DirectEquality"/> of the first non-null <see cref="JNode"/> as comparison kind.</summary>
    public static bool operator ==(object? a, JNode? b) => b == a;

    /// <summary>Determines whether the specified node and object are considered unequal. It uses <see cref="JNodeOptions.DirectEquality"/> of the first non-null <see cref="JNode"/> as comparison kind.</summary>
    public static bool operator !=(JNode? a, object? b) => !(a == b);

    /// <summary>Determines whether the specified object and node are considered unequal. It uses <see cref="JNodeOptions.DirectEquality"/> of the first non-null <see cref="JNode"/> as comparison kind.</summary>
    public static bool operator !=(object? a, JNode? b) => !(b == a);

    //[return: NotNullIfNotNull(nameof(node))]
    //private protected static object? ToJNodeOrValue(JsonNode? node, JNodeOptions options) =>
    //    node.ToJNodeOrValue(options);

    private protected abstract class MetaJsonNodeBase(E expression, JNode value)
        : dobject(expression, BindingRestrictions.Empty, value)
    {
        protected new JNode Value => (JNode)base.Value!;

        protected abstract E ExprSelf();

        protected abstract dobject BindSelf();

        protected abstract E ExprNode();

        protected abstract dobject BindNode();

        protected static E ExprOther(dobject other) =>
            other.Expression.EConvertIfNeeded<JNode>();

        protected static dobject BindOther(dobject other) =>
            ExprOther(other).ToDObject((JNode?)other.Value);

        protected static E ExprOtherNode(dobject other) =>
            other.Expression.EConvertIfNeeded<JNode>().EProperty(PNodeUntyped.Getter.Method);

        protected static dobject BindOtherNode(dobject other) =>
            ExprOtherNode(other).ToDObject(((JNode?)other.Value)?.NodeUntyped);

        protected BindingRestrictions GetRestrictions() =>
            BindingRestrictions.GetTypeRestriction(Expression, LimitType);

        public override dobject BindInvokeMember(InvokeMemberBinder binder, dobject[] args) =>
            binder.Name switch {
                nameof(GetPath) =>
                    CallNodeMethod(PNodeGetPath, [ ]),
                nameof(Clone) =>
                    CallSelfMethod(PClone, [ ]),
                nameof(Equals) =>
                    CallSelfMethod(PEquals, args.SelectExpressions()),
                nameof(GetHashCode) =>
                    CallSelfMethod(PGetHashCode, [ ]),
                nameof(ToJsonString) =>
                    CallNodeMethod(PNodeToJsonString, args.SelectExpressions()),
                _ =>
                    CallSelfMethod(binder, args),
            };

        public override dobject BindBinaryOperation(BinaryOperationBinder binder, dobject arg) =>
            binder.Operation switch {
                ExpressionType.Equal =>
                    CallSelfMethod(PEquals, [ ExprOther(arg) ]),
                ExpressionType.NotEqual =>
                    CallSelfMethod(PEquals, [ ExprOther(arg) ], wrap: e => e.ENot()),
                _ =>
                    BindSelf().Fallback(binder, BindOther(arg)),
            };

        protected dobject CallMethod(E? instance, MethodRef m,
            E[] parameters, Type[]? genericTypes = null, Func<E, E>? wrap = null)
        {
            E expr = instance.ECall(m.GetMethod(genericTypes), parameters);
            expr = wrap?.Invoke(expr) ?? expr;
            expr = m.IsVoid ? E.Block(typeof(object), expr, E.Constant(null)) : expr.EConvertIfNeeded<object>();
            return new(expr, GetRestrictions());
        }

        protected dobject CallStaticMethod(MethodRef m,
            E[] parameters, Type[]? genericTypes = null, Func<E, E>? wrap = null) =>
            CallMethod(null, m, parameters, genericTypes, wrap);

        protected abstract dobject CallSelfMethod(MethodRef m,
            E[] parameters, Type[]? genericTypes = null, Func<E, E>? wrap = null);

        protected abstract dobject CallSelfMethod(InvokeMemberBinder binder, dobject[] args);

        protected abstract dobject CallNodeMethod(MethodRef m,
            E[] parameters, Type[]? genericTypes = null, Func<E, E>? wrap = null);

        protected abstract dobject CallNodeMethod(InvokeMemberBinder binder, dobject[] args);
    }
}