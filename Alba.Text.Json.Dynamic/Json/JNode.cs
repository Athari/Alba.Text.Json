using System.Dynamic;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Alba.Text.Json.Dynamic;

[JsonConverter(typeof(JNodeConverter))]
public abstract partial class JNode(JNodeOptions? options = null) : IEquatable<JNode>, ICloneable
{
    private static readonly MethodRef PClone = MethodRef.Of((JNode o) => o.Clone());
    private static readonly MethodRef PEquals = MethodRef.Of((JNode o) => o.Equals(null));
    private static readonly MethodRef PGetHashCode = MethodRef.Of((JNode o) => o.GetHashCode());
    private static readonly MethodRef PToJsonString = MethodRef.Of((JNode o) => o.ToJsonString(null));

    private static readonly PropertyRef PNodeUntyped = PropertyRef.Of((JNode o) => o.NodeUntyped);

    internal readonly JNodeOptions Options = options ?? JNodeOptions.Default;

    internal abstract JsonNode NodeUntyped { get; }

    public JNode Clone() =>
        (JNode)JOperations.NodeToDynamicNodeOrValue(JOperations.DeepClone(NodeUntyped), Options);

    object ICloneable.Clone() =>
        Clone();

    public bool Equals(JNode? o) =>
        JOperations.DeepEquals(NodeUntyped, o?.NodeUntyped, Options);

    public sealed override bool Equals(object? obj) =>
        obj is JNode o && Equals(o);

    public sealed override int GetHashCode() =>
        JOperations.GetDeepHashCode(Options.MaxHashCodeValueCount, Options.MaxHashCodeDepth);

    public string ToJsonString(JsonSerializerOptions? opts) =>
        NodeUntyped.ToJsonString(opts);

    public static bool operator ==(JNode? a, JNode? b) =>
        JOperations.DeepEquals(a?.NodeUntyped, b?.NodeUntyped, a?.Options ?? b?.Options ?? JNodeOptions.Default);

    public static bool operator !=(JNode? a, JNode? b) =>
        !(a == b);

    private protected abstract class MetaJsonNodeBase(E expression, JNode value)
        : dobject(expression, BindingRestrictions.Empty, value)
    {
        protected new JNode Value => (JNode)base.Value!;

        protected abstract E ExprSelf();

        protected abstract dobject BindSelf();

        protected abstract E ExprNode();

        protected abstract dobject BindNode();

        protected E ExprOther(dobject other) =>
            other.Expression.EConvertIfNeeded<JNode>();

        protected dobject BindOther(dobject other) =>
            ExprOther(other).ToDObject(Value);

        protected E ExprOtherNode(dobject other) =>
            other.Expression.EConvertIfNeeded<JNode>().EProperty(PNodeUntyped.Getter);

        protected dobject BindOtherNode(dobject other) =>
            ExprOtherNode(other).ToDObject((other.Value as JNode)?.NodeUntyped);

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
                    CallSelfMethod(PToJsonString, args.SelectExpressions()),
                _ =>
                    CallSelfMethod(binder, args),
            };

        public override dobject BindBinaryOperation(BinaryOperationBinder binder, dobject arg) =>
            binder.Operation switch {
                ExpressionType.Equal =>
                    CallSelfMethod(PEquals, [ ExprOther(arg) ], wrap: e => e.EConvert<object>()),
                ExpressionType.NotEqual =>
                    CallSelfMethod(PEquals, [ ExprOther(arg) ], wrap: e => e.ENot().EConvert<object>()),
                _ =>
                    BindSelf().Fallback(binder, BindOther(arg)),
            };

        protected dobject CallMethod(E? instance, MethodRef m,
            E[] parameters, Type[]? genericTypes = null, Func<E, E>? wrap = null)
        {
            var expr = instance.ECall(m.GetMethod(genericTypes), parameters).EBlockEmptyIfNeeded(m.IsVoid);
            return new(wrap?.Invoke(expr) ?? expr, GetRestrictions());
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