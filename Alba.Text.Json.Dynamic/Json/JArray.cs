using System.Collections;
using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed class JArray(JsonArray source, JNodeOptions? options = null)
    : JNode<JsonArray>(source, options), IDynamicMetaObjectProvider,
        IEnumerable<object?>, IEquatable<JArray>
{
    private static readonly MethodRef PGetInt = MethodRef.Of((JArray o) => o.Get(0));
    private static readonly MethodRef PGetIndex = MethodRef.Of((JArray o) => o.Get(Index.Start));
    private static readonly MethodRef PSetInt = MethodRef.Of((JArray o) => o.Set(0, MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PSetIndex = MethodRef.Of((JArray o) => o.Set(Index.Start, MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PAdd = MethodRef.Of((JArray o) => o.Add(MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PInsertInt = MethodRef.Of((JArray o) => o.Insert(0, MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PInsertIndex = MethodRef.Of((JArray o) => o.Insert(Index.Start, MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PRemove = MethodRef.Of((JArray o) => o.Remove((JNode?)null));
    private static readonly MethodRef PRemoveNode = MethodRef.Of((JArray o) => o.Remove((JsonNode?)null));
    private static readonly MethodRef PRemoveValue = MethodRef.Of((JArray o) => o.Remove(MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PRemoveAtInt = MethodRef.Of((JArray o) => o.RemoveAt(0));
    private static readonly MethodRef PRemoveAtIndex = MethodRef.Of((JArray o) => o.RemoveAt(Index.Start));
    private static readonly MethodRef PContains = MethodRef.Of((JArray o) => o.Contains((JNode?)null));
    private static readonly MethodRef PContainsNode = MethodRef.Of((JArray o) => o.Contains((JsonNode?)null));
    private static readonly MethodRef PContainsValue = MethodRef.Of((JArray o) => o.Contains(MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PIndexOf = MethodRef.Of((JArray o) => o.IndexOf((JNode?)null));
    private static readonly MethodRef PIndexOfNode = MethodRef.Of((JArray o) => o.IndexOf((JsonNode?)null));
    private static readonly MethodRef PIndexOfValue = MethodRef.Of((JArray o) => o.IndexOf(MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PGetEnumerator = MethodRef.Of((JArray o) => o.GetEnumerator());

    private static readonly PropertyRef PNodeCount = PropertyRef.Of((JsonArray o) => o.Count);
    private static readonly MethodRef PNodeClear = MethodRef.Of((JsonArray o) => o.Clear());

    public object? this[int index] {
        get => Get(index);
        set => Set(index, value);
    }

    public object? this[Index index] {
        get => JOperations.NodeToDynamicNodeOrValue(Node[index], Options);
        set => Node[index] = JOperations.ValueToNewNode(value, Node.Options);
    }

    public int Count() =>
        Node.Count;

    public object? Get(int index) =>
        JOperations.NodeToDynamicNodeOrValue(Node[index], Options);

    public object? Get(Index index) =>
        JOperations.NodeToDynamicNodeOrValue(Node[index], Options);

    public void Set<T>(int index, T value) =>
        Node[index] = JOperations.ValueToNewNode(value, Node.Options);

    public void Set<T>(Index index, T value) =>
        Node[index] = JOperations.ValueToNewNode(value, Node.Options);

    public void Add<T>(T value) =>
        Node.Add(JOperations.ValueToNewNode(value, Node.Options));

    public void Insert<T>(int index, T value) =>
        Node.Insert(index, JOperations.ValueToNewNode(value, Node.Options));

    public void Insert<T>(Index index, T value) =>
        Node.Insert(index.GetOffset(Node.Count), JOperations.ValueToNewNode(value, Node.Options));

    public bool Remove(JsonNode? item) =>
        Node.Remove(item);

    public bool Remove(JNode? item) =>
        Remove(item?.NodeUntyped);

    public bool Remove<T>(T item) =>
        JOperations.ValueToValueNode(item, out var v, Node.Options) && Remove(v);

    public void RemoveAt(int index) =>
        Node.RemoveAt(index);

    public void RemoveAt(Index index) =>
        Node.RemoveAt(index.GetOffset(Node.Count));

    public void Clear() =>
        Node.Clear();

    public bool Contains(JsonNode? item) =>
        Node.Any(i => JOperations.ShallowEquals(i, item, Options));

    public bool Contains(JNode? item) =>
        Contains(item?.NodeUntyped);

    public bool Contains<T>(T item) =>
        JOperations.ValueToValueNode(item, out var v, Node.Options) && Contains(v);

    public int IndexOf(JsonNode? item) =>
        Node.IndexOf(i => JOperations.ShallowEquals(i, item, Options));

    public int IndexOf(JNode? item) =>
        IndexOf(item?.NodeUntyped);

    public int IndexOf<T>(T item) =>
        JOperations.ValueToValueNode(item, out var v, Node.Options) ? IndexOf(v) : -1;

    public new JArray Clone() =>
        (JArray)base.Clone();

    public bool Equals(JArray? other) =>
        base.Equals(other);

    public IEnumerator<object?> GetEnumerator() =>
        Node.Select(p => JOperations.NodeToDynamicNodeOrValue(p, Options)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public dobject GetMetaObject(E expression) => new MetaJArray(expression, this);

    private sealed class MetaJArray(E expression, JArray dynamicValue)
        : MetaJNode<JArray>(expression, dynamicValue)
    {
        public override dobject BindGetIndex(GetIndexBinder binder, dobject[] indexes) =>
            CallSelfMethod(SelectIndexMethod(indexes, PGetInt, PGetIndex),
                indexes.SelectExpressions());

        public override dobject BindSetIndex(SetIndexBinder binder, dobject[] indexes, dobject value) =>
            CallSelfMethod(SelectIndexMethod(indexes, PSetInt, PSetIndex),
                [ indexes[0].Expression, value.GetTypedExpression() ], [ value.LimitType ]);

        public override dobject BindDeleteIndex(DeleteIndexBinder binder, dobject[] indexes) =>
            CallSelfMethod(SelectIndexMethod(indexes, PRemoveAtInt, PRemoveAtIndex),
                indexes.SelectExpressions());

        public override dobject BindInvokeMember(InvokeMemberBinder binder, dobject[] args) =>
            binder.Name switch {
                nameof(Count) =>
                    ExprNode().EProperty(PNodeCount.Getter).ToDObject(Value.Node.Count),
                nameof(Add) =>
                    CallSelfMethod(PAdd, [ args.Single().GetTypedExpression() ], [ args[0].LimitType ]),
                nameof(Insert) =>
                    CallSelfMethod(
                        SelectIndexMethod(Ensure.Count(args, 2), PInsertInt, PInsertIndex),
                        [ args[0].Expression, args[1].GetTypedExpression() ], [ args[1].LimitType ]),
                nameof(Remove) =>
                    CallSelfMethod(
                        SelectItemMethod(Ensure.Count(args, 1), PRemove, PRemoveNode, PRemoveValue),
                        [ args[0].GetTypedExpression() ], [ args[0].LimitType ], wrap: e => e.EConvert<object>()),
                nameof(RemoveAt) =>
                    CallSelfMethod(
                        SelectIndexMethod(Ensure.Count(args, 1), PRemoveAtInt, PRemoveAtIndex),
                        args.SelectExpressions()),
                nameof(Contains) =>
                    CallSelfMethod(
                        SelectItemMethod(Ensure.Count(args, 1), PContains, PContainsNode, PContainsValue),
                        [ args[0].GetTypedExpression() ], [ args[0].LimitType ], wrap: e => e.EConvert<object>()),
                nameof(IndexOf) =>
                    CallSelfMethod(
                        SelectItemMethod(Ensure.Count(args, 1), PIndexOf, PIndexOfNode, PIndexOfValue),
                        [ args[0].GetTypedExpression() ], [ args[0].LimitType ], wrap: e => e.EConvert<object>()),
                nameof(Clear) =>
                    CallNodeMethod(PNodeClear, [ ]),
                nameof(GetEnumerator) =>
                    CallSelfMethod(PGetEnumerator, [ ], wrap: e => e.EConvert<object>()),
                _ =>
                    base.BindInvokeMember(binder, args),
            };

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var count = Value.Node.Count;
            for (int i = 0; i < count; i++)
                yield return i.ToString();
        }

        private static MethodRef SelectIndexMethod(dobject[] indexes,
            MethodRef intMethod, MethodRef indexMethod) =>
            indexes switch {
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

        private static MethodRef SelectItemMethod(dobject[] args,
            MethodRef method, MethodRef nodeMethod, MethodRef valueMethod) =>
            args switch {
                [ var index ] => index switch {
                    { HasValue: true, Value: var v } => v switch {
                        JNode => method,
                        JsonNode => nodeMethod,
                        _ => valueMethod,
                    },
                    { LimitType: var t } =>
                        t == typeof(JNode) ? method :
                        t == typeof(JsonNode) ? nodeMethod :
                        valueMethod,
                },
                _ => throw new ArgumentException("One value expected.", nameof(args)),
            };

        [SuppressMessage("ReSharper", "NotResolvedInText")]
        private static ArgumentException IndexArgumentType() => new("Argument index must be Int32 or Index.", "index");
    }
}