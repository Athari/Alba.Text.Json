using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

/// <summary>Generic base of dynamic adapters for <see cref="JsonNode"/> types with support for dynamic dispatch via <see cref="IDynamicMetaObjectProvider"/>.</summary>
/// <typeparam name="TNode">Type of <see cref="JsonNode"/> to wrap.</typeparam>
/// <param name="source"><see cref="JsonNode"/> to wrap.</param>
/// <param name="options">Options to control the behavior.</param>
public class JNode<TNode>(TNode source, JNodeOptions? options = null)
    : JNode(options)
    where TNode : JsonNode
{
    private static readonly FieldRef PNode = FieldRef.Of((JNode<TNode> o) => o.Node);

    internal readonly TNode Node = source ?? throw new ArgumentNullException(nameof(source));

    internal sealed override JsonNode NodeUntyped => Node;

    private protected abstract class MetaJNode<TDynamicNode>(E expression, TDynamicNode dynamicValue)
        : MetaJsonNodeBase(expression, dynamicValue)
        where TDynamicNode : JNode<TNode>
    {
        protected new TDynamicNode Value => (TDynamicNode)base.Value;

        protected sealed override E ExprSelf() =>
            Expression.EConvertIfNeeded<TDynamicNode>();

        protected sealed override dobject BindSelf() =>
            ExprSelf().ToDObject(Value);

        protected sealed override E ExprNode() =>
            ExprSelf().EField(PNode.Field);

        protected sealed override dobject BindNode() =>
            ExprNode().ToDObject(Value.Node);

        public override dobject BindInvoke(InvokeBinder binder, dobject[] args) =>
            BindNode();

        protected sealed override dobject CallSelfMethod(MethodRef m,
            E[] parameters, Type[]? genericTypes = null, Func<E, E>? wrap = null) =>
            CallMethod(ExprSelf(), m, parameters, genericTypes, wrap);

        protected sealed override dobject CallSelfMethod(InvokeMemberBinder binder, dobject[] args) =>
            BindSelf().Fallback(binder, args);

        protected sealed override dobject CallNodeMethod(MethodRef m,
            E[] parameters, Type[]? genericTypes = null, Func<E, E>? wrap = null) =>
            CallMethod(ExprNode(), m, parameters, genericTypes, wrap);

        protected sealed override dobject CallNodeMethod(InvokeMemberBinder binder, dobject[] args) =>
            BindNode().Fallback(binder, args);
    }
}