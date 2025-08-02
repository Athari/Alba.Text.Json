using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public class JNode<TNode>(TNode source, JNodeOptions? options = null)
    : JNode(options)
    where TNode : JsonNode
{
    private static readonly FieldRef DynamicJsonNodeOfTNode_Node_Field = FieldRef.Of((JNode<TNode> o) => o.Node);

    internal readonly TNode Node = source ?? throw new ArgumentNullException(nameof(source));

    internal sealed override JsonNode NodeUntyped => Node;

    private protected abstract class MetaJNode<TDynamicNode>(E expression, TDynamicNode dynamicValue)
        : MetaJsonNodeBase(expression, dynamicValue)
        where TDynamicNode : JNode<TNode>
    {
        protected new TDynamicNode Value => (TDynamicNode)base.Value!;

        protected E ExprSelf() => Expression.EConvertIfNeeded<TDynamicNode>();

        protected E ExprNode() => ExprSelf().EField(DynamicJsonNodeOfTNode_Node_Field.Field);

        protected dobject BindNode() => ExprNode().ToDObject(Value.Node);

        public override dobject BindInvoke(InvokeBinder binder, dobject[] args) =>
            BindNode();

        public override dobject BindInvokeMember(InvokeMemberBinder binder, dobject[] args) =>
            CallNodeMethod(binder, args);

        protected dobject CallMethod(MethodRef m, E[] parameters, Type[]? genericTypes = null) =>
            new(ExprSelf().ECall(m.GetMethod(genericTypes), parameters).EBlockEmptyIfNeeded(m.IsVoid), GetRestrictions());

        protected dobject CallNodeMethod(InvokeMemberBinder binder, dobject[] args) =>
            BindNode().Fallback(binder, args);
    }
}