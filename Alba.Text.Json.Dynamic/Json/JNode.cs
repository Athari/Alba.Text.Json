using System.Dynamic;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Alba.Text.Json.Dynamic;

[JsonConverter(typeof(JNodeConverter))]
public abstract partial class JNode(JNodeOptions? options = null) : IEquatable<object?>
{
    private static readonly MethodRef PClone = MethodRef.Of((JNode o) => o.Clone());
    private static readonly MethodRef PEquals = MethodRef.Of((JNode o) => o.Equals(0));
    private static readonly MethodRef PGetHashCode = MethodRef.Of((JNode o) => o.GetHashCode());

    private static readonly PropertyRef PNodeUntyped = PropertyRef.Of((JNode o) => o.NodeUntyped);
    private static readonly MethodRef PNodeToJsonString = MethodRef.Of((JsonNode o) => o.ToJsonString(null));

    internal readonly JNodeOptions Options = options ?? JNodeOptions.Default;

    internal abstract JsonNode NodeUntyped { get; }

    public JNode Clone() =>
        (JNode)JsonNode.ToJNodeOrValue(JsonNode.DeepClone(NodeUntyped), Options);

    public sealed override bool Equals(object? o) =>
        JsonNode.Equals(NodeUntyped, o, Options.DirectEquality, Options);

    public sealed override int GetHashCode() =>
        JsonNode.ToHashCode(NodeUntyped, Options.DirectEquality, Options);

    public string ToJsonString(JsonSerializerOptions? opts) =>
        NodeUntyped.ToJsonString(opts);

    public static bool operator ==(JNode? a, object? b)
    {
        var options = a?.Options ?? (b as JNode)?.Options ?? JNodeOptions.Default;
        return JsonNode.Equals(a?.NodeUntyped, b, options.DirectEquality, options);
    }

    public static bool operator ==(object? b, JNode? a) => a == b;
    public static bool operator !=(JNode? a, object? b) => !(a == b);
    public static bool operator !=(object? b, JNode? a) => !(a == b);

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