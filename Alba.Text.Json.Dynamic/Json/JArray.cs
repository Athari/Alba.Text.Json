using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JArray(JsonArray source, JNodeOptions? options = null)
    : JNode<JsonArray>(source, options), IDynamicMetaObjectProvider
{
    private static readonly MethodRef PGetInt = MethodRef.Of((JArray o) => o.Get(0));
    private static readonly MethodRef PGetIndex = MethodRef.Of((JArray o) => o.Get(Index.Start));
    private static readonly MethodRef PSetInt = MethodRef.Of((JArray o) => o.Set(0, MethodKey.GetT(0)));
    private static readonly MethodRef PSetIndex = MethodRef.Of((JArray o) => o.Set(Index.Start, MethodKey.GetT(0)));
    private static readonly MethodRef PAdd = MethodRef.Of((JArray o) => o.Add(MethodKey.GetT(0)));
    private static readonly MethodRef PInsertInt = MethodRef.Of((JArray o) => o.Insert(0, MethodKey.GetT(0)));
    private static readonly MethodRef PInsertIndex = MethodRef.Of((JArray o) => o.Insert(Index.Start, MethodKey.GetT(0)));
    private static readonly MethodRef PRemove = MethodRef.Of((JArray o) => o.Remove(MethodKey.GetT(0)));
    private static readonly MethodRef PRemoveAtInt = MethodRef.Of((JArray o) => o.RemoveAt(0));
    private static readonly MethodRef PRemoveAtIndex = MethodRef.Of((JArray o) => o.RemoveAt(Index.Start));
    private static readonly MethodRef PContains = MethodRef.Of((JArray o) => o.Contains(MethodKey.GetT(0)));
    private static readonly MethodRef PIndexOf = MethodRef.Of((JArray o) => o.IndexOf(MethodKey.GetT(0)));
    private static readonly MethodRef PGetEnumerator = MethodRef.Of((JArray o) => o.GetEnumerator());

    private static readonly PropertyRef PNodeCount = PropertyRef.Of((JsonArray o) => o.Count);
    private static readonly MethodRef PNodeClear = MethodRef.Of((JsonArray o) => o.Clear());

    public object? this[int index] {
        get => Get(index);
        set => Set(index, value);
    }

    public object? this[Index index] {
        get => JsonNode.ToJNodeOrValue(Node[index], Options);
        set => Node[index] = ValueTypeExts.ToNewJsonNode(value, Node.Options);
    }

    public int Count =>
        Node.Count;

    private object? Get(int index) =>
        JsonNode.ToJNodeOrValue(Node[index], Options);

    private object? Get(Index index) =>
        JsonNode.ToJNodeOrValue(Node[index], Options);

    private void Set<T>(int index, T value) =>
        Node[index] = ValueTypeExts.ToNewJsonNode(value, Node.Options);

    private void Set<T>(Index index, T value) =>
        Node[index] = ValueTypeExts.ToNewJsonNode(value, Node.Options);

    public void Add<T>(T value) =>
        Node.Add(ValueTypeExts.ToNewJsonNode(value, Node.Options));

    public void Insert<T>(int index, T value) =>
        Node.Insert(index, ValueTypeExts.ToNewJsonNode(value, Node.Options));

    public void Insert<T>(Index index, T value) =>
        Node.Insert(index.GetOffset(Node.Count), ValueTypeExts.ToNewJsonNode(value, Node.Options));

    public bool Remove<T>(T item) =>
        JsonNodeList.Remove(Node, item, Options);

    public void RemoveAt(int index) =>
        Node.RemoveAt(index);

    public void RemoveAt(Index index) =>
        Node.RemoveAt(index.GetOffset(Node.Count));

    public void Clear() =>
        Node.Clear();

    public bool Contains<T>(T item) =>
        JsonNodeList.Contains(Node, item, Options);

    public int IndexOf<T>(T item) =>
        JsonNodeList.IndexOf(Node, item, Options);

    public new JArray Clone() =>
        (JArray)base.Clone();

    public IEnumerator<object?> GetEnumerator() =>
        Node.Select(n => JsonNode.ToJNodeOrValue(n, Options)).GetEnumerator();

    public dobject GetMetaObject(E expression) => new MetaJArray(expression, this);

    private sealed class MetaJArray(E expression, JArray dynamicValue)
        : MetaJNode<JArray>(expression, dynamicValue)
    {
        public override dobject BindGetIndex(GetIndexBinder binder, dobject[] indexes) =>
            CallSelfMethod(SelectIndexMethod(indexes, 1, PGetInt, PGetIndex),
                indexes.SelectExpressions());

        public override dobject BindSetIndex(SetIndexBinder binder, dobject[] indexes, dobject value) =>
            CallSelfMethod(SelectIndexMethod(indexes, 1, PSetInt, PSetIndex),
                [ indexes[0].Expression, value.TypedExpression ], [ value.LimitType ]);

        public override dobject BindDeleteIndex(DeleteIndexBinder binder, dobject[] indexes) =>
            CallSelfMethod(SelectIndexMethod(indexes, 1, PRemoveAtInt, PRemoveAtIndex),
                indexes.SelectExpressions());

        public override dobject BindInvokeMember(InvokeMemberBinder binder, dobject[] args) =>
            binder.Name switch {
                nameof(Count) =>
                    ExprNode().EProperty(PNodeCount.Getter.Method).ToDObject(Value.Node.Count),
                nameof(Add) =>
                    CallSelfMethod(PAdd, args.SelectTypedExpressions(), args.SelectTypes()),
                nameof(Insert) =>
                    CallSelfMethod(
                        SelectIndexMethod(args, 2, PInsertInt, PInsertIndex),
                        [ args[0].Expression, args[1].TypedExpression ], args.SelectType(1)),
                nameof(Remove) =>
                    CallSelfMethod(PRemove, args.SelectTypedExpressions(), args.SelectTypes()),
                nameof(RemoveAt) =>
                    CallSelfMethod(
                        SelectIndexMethod(args, 1, PRemoveAtInt, PRemoveAtIndex),
                        args.SelectExpressions()),
                nameof(Contains) =>
                    CallSelfMethod(PContains, args.SelectTypedExpressions(), args.SelectTypes()),
                nameof(IndexOf) =>
                    CallSelfMethod(PIndexOf, args.SelectTypedExpressions(), args.SelectTypes()),
                nameof(Clear) =>
                    CallNodeMethod(PNodeClear, [ ]),
                nameof(GetEnumerator) =>
                    CallSelfMethod(PGetEnumerator, [ ]),
              #if JSON10_0_OR_GREATER
                nameof(RemoveAll) =>
                    CallSelfMethod(PRemoveAll, args.SelectExpressions()),
                nameof(RemoveRange) =>
                    CallNodeMethod(PNodeRemoveRange, args.SelectExpressions()),
              #endif
                _ =>
                    base.BindInvokeMember(binder, args),
            };

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var count = Value.Node.Count;
            for (int i = 0; i < count; i++)
                yield return $"{i}";
        }

        private static MethodRef SelectIndexMethod(dobject[] indexes, int count,
            MethodRef intMethod, MethodRef indexMethod) =>
            Ensure.Count(indexes, count) switch {
                [ var index, .. ] => index switch {
                    { HasValue: true, Value: var v } => v switch {
                        int => intMethod,
                        Index => indexMethod,
                        _ => throw IndexArgumentType(),
                    },
                    { LimitType: var t } =>
                        t == typeof(int) ? intMethod :
                        t == typeof(Index) ? indexMethod :
                        throw IndexArgumentType(),
                },
                _ => throw new ArgumentException("One index expected.", nameof(indexes)),
            };

        [SuppressMessage("ReSharper", "NotResolvedInText")]
        private static ArgumentException IndexArgumentType() => new("Argument index must be Int32 or Index.", "index");
    }
}