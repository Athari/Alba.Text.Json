using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed class JArray(JsonArray source, JNodeOptions? options = null)
    : JNode<JsonArray>(source, options), IDynamicMetaObjectProvider
{
    private static readonly MethodRef GetItem_Method = MethodRef.Of((JArray a) => a[0]);
    private static readonly MethodRef SetItem_Method = MethodRef.Of(MethodKey.RegularInstance<JArray, Action<int, object?>>("set_Item"));

    public object? this[int index] {
        get => JOperations.NodeToDynamicNodeOrValue(Node[index], Options);
        set => Node[index] = JOperations.ValueToNode(value, Node.Options);
    }

    public dobject GetMetaObject(E expression) => new MetaJArray(expression, this);

    private class MetaJArray(E expression, JArray dynamicValue)
        : MetaJNode<JArray>(expression, dynamicValue)
    {
        public override dobject BindGetIndex(GetIndexBinder binder, dobject[] indexes) =>
            CallMethod(GetItem_Method, [ indexes.Single().Expression ]);

        public override dobject BindSetIndex(SetIndexBinder binder, dobject[] indexes, dobject value) =>
            CallMethod(SetItem_Method, [ indexes.Single().Expression, value.Expression ], [ value.LimitType ]);

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var count = Value.Node.Count;
            for (int i = 0; i < count; i++)
                yield return i.ToString();
        }
    }
}