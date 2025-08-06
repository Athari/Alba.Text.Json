using System.Dynamic;
using System.Text.Json.Nodes;
using static Alba.Text.Json.Dynamic.JOperations;

namespace Alba.Text.Json.Dynamic;

public sealed partial class JObject(JsonObject source, JNodeOptions? options = null)
    : JNode<JsonObject>(source, options), IDynamicMetaObjectProvider
{
    private static readonly MethodRef PGet = MethodRef.Of((JObject o) => o.Get(""));
    private static readonly MethodRef PSet = MethodRef.Of((JObject o) => o.Set("", MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PAdd = MethodRef.Of((JObject o) => o.Add("", MethodKey.GetT<object?>(0)));
    private static readonly MethodRef PGetEnumerator = MethodRef.Of((JObject o) => o.GetEnumerator());

    private static readonly PropertyRef PNodeCount = PropertyRef.Of((JsonObject o) => o.Count);
    private static readonly MethodRef PNodeClear = MethodRef.Of((JsonObject o) => o.Clear());
    private static readonly MethodRef PNodeContainsKey = MethodRef.Of((JsonObject o) => o.ContainsKey(""));
    private static readonly MethodRef PNodeRemove = MethodRef.Of((JsonObject o) => o.Remove(""));

    public object? this[string key] {
        get => Get(key);
        set => Set(key, value);
    }

    public int Count =>
        Node.Count;

    private object? Get(string name) =>
        JsonNodeToJNodeOrValue(Node[name], Options);

    public bool TryGet(string name, out object? value) =>
        Node.TryGetPropertyValue(name, out var node) switch {
            var r => (value = r ? JsonNodeToJNodeOrValue(node, Options) : null, r).r,
        };

    private void Set<T>(string name, T value) =>
        Node[name] = ValueToNewJsonNode(value, Node.Options);

    public void Add<T>(string name, T value) =>
        Node.Add(name, ValueToNewJsonNode(value, Node.Options));

    public bool Remove(string name) =>
        Node.Remove(name);

    public void Clear() =>
        Node.Clear();

    public bool ContainsKey(string name) =>
        Node.ContainsKey(name);

    public new JObject Clone() =>
        (JObject)base.Clone();

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() =>
        Node.Select(p => new KeyValuePair<string, object?>(p.Key, JsonNodeToJNodeOrValue(p.Value, Options)))
            .GetEnumerator();

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
                    CallSelfMethod(PAdd, args.SelectTypedExpressions(), args.SelectType(1)),
                nameof(Remove) =>
                    CallNodeMethod(PNodeRemove, args.SelectExpressions()),
                nameof(Clear) =>
                    CallNodeMethod(PNodeClear, [ ]),
                nameof(ContainsKey) =>
                    CallNodeMethod(PNodeContainsKey, args.SelectExpressions()),
                nameof(GetEnumerator) =>
                    CallSelfMethod(PGetEnumerator, [ ]),
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