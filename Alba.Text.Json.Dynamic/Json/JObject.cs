using System.Collections;
using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed class JObject(JsonObject source, JNodeOptions? options = null)
    : JNode<JsonObject>(source, options), IDynamicMetaObjectProvider,
        IEnumerable<KeyValuePair<string, object?>>, IEquatable<JObject>
{
    private static readonly MethodRef PGet = MethodRef.Of((JObject o) => o.Get(""));
    private static readonly MethodRef PSet = MethodRef.Of((JObject o) => o.Set("", MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PAdd = MethodRef.Of((JObject o) => o.Add("", MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PGetEnumerator = MethodRef.Of((JObject o) => o.GetEnumerator());

    private static readonly PropertyRef PNodeCount = PropertyRef.Of((JsonObject o) => o.Count);
    private static readonly MethodRef PNodeClear = MethodRef.Of((JsonObject o) => o.Clear());
    private static readonly MethodRef PNodeContainsKey = MethodRef.Of((JsonObject o) => o.ContainsKey(""));
    private static readonly MethodRef PNodeRemove = MethodRef.Of((JsonObject o) => o.Remove(""));

    public int Count() =>
        Node.Count;

    public object? Get(string name) =>
        JOperations.NodeToDynamicNodeOrValue(Node[name], Options);

    public void Set<T>(string name, T value) =>
        Node[name] = JOperations.ValueToNewNode(value, Node.Options);

    public void Add<T>(string name, T value) =>
        Node.Add(name, JOperations.ValueToNewNode(value, Node.Options));

    public bool Remove(string name) =>
        Node.Remove(name);

    public void Clear() =>
        Node.Clear();

    public bool ContainsKey(string name) =>
        Node.ContainsKey(name);

    public new JObject Clone() =>
        (JObject)base.Clone();

    public bool Equals(JObject? other) =>
        base.Equals(other);

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() =>
        Node.Select(p => new KeyValuePair<string, object?>(p.Key, JOperations.NodeToDynamicNodeOrValue(p.Value, Options)))
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public dobject GetMetaObject(E expression) =>
        new MetaJObject(expression, this);

    private sealed class MetaJObject(E expression, JObject dynamicValue)
        : MetaJNode<JObject>(expression, dynamicValue)
    {
        public override dobject BindGetIndex(GetIndexBinder binder, dobject[] indexes) =>
            CallSelfMethod(PGet, [ indexes.Single().Expression ]);

        public override dobject BindSetIndex(SetIndexBinder binder, dobject[] indexes, dobject value) =>
            CallSelfMethod(PSet, [ indexes.Single().Expression, value.GetTypedExpression() ], [ value.LimitType ]);

        public override dobject BindDeleteIndex(DeleteIndexBinder binder, dobject[] indexes) =>
            CallNodeMethod(PNodeRemove, [ indexes.Single().Expression ]);

        public override dobject BindGetMember(GetMemberBinder binder) =>
            CallSelfMethod(PGet, [ binder.Name.EConst() ]);

        public override dobject BindSetMember(SetMemberBinder binder, dobject value) =>
            CallSelfMethod(PSet, [ binder.Name.EConst(), value.GetTypedExpression() ], [ value.LimitType ]);

        public override dobject BindDeleteMember(DeleteMemberBinder binder) =>
            CallNodeMethod(PNodeRemove, [ binder.Name.EConst() ]);

        public override dobject BindInvokeMember(InvokeMemberBinder binder, dobject[] args) =>
            binder.Name switch {
                nameof(Count) =>
                    ExprNode().EProperty(PNodeCount.Getter).ToDObject(Value.Node.Count),
                nameof(Add) =>
                    CallSelfMethod(PAdd, args.SelectExpressions(), [ args[1].LimitType ]),
                nameof(Remove) =>
                    CallNodeMethod(PNodeRemove, args.SelectExpressions(), wrap: e => e.EConvert<object>()),
                nameof(Clear) =>
                    CallNodeMethod(PNodeClear, [ ]),
                nameof(ContainsKey) =>
                    CallNodeMethod(PNodeContainsKey, args.SelectExpressions(), wrap: e => e.EConvert<object>()),
                nameof(GetEnumerator) =>
                    CallSelfMethod(PGetEnumerator, [ ], wrap: e => e.EConvert<object>()),
                _ =>
                    base.BindInvokeMember(binder, args),
            };

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach ((string key, JsonNode? _) in Value.Node)
                yield return key;
        }
    }
}