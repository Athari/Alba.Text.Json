using System.Dynamic;
using System.Globalization;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed class JArray(JsonArray source, JNodeOptions? options = null)
    : JNode<JsonArray>(source, options), IDynamicMetaObjectProvider
{
    public dobject GetMetaObject(E expression) => new MetaJArray(expression, this);

    private class MetaJArray(E expression, JArray dynamicValue)
        : MetaJNode<JArray>(expression, dynamicValue)
    {
        public override dobject BindGetIndex(GetIndexBinder binder, dobject[] indexes) =>
            CallStaticMethod(JOperations.JsonArray_GetIndex_Method,
                [ ExprSelf(), indexes.Single().Expression ]);

        public override dobject BindSetIndex(SetIndexBinder binder, dobject[] indexes, dobject value) =>
            CallStaticMethod(JOperations.JsonArray_SetIndex_Method,
                [ ExprSelf(), indexes.Single().Expression, value.Expression ], [ value.LimitType ]);

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var count = Value.Node.Count;
            for (int i = 0; i < count; i++)
                yield return i.ToString("d", CultureInfo.InvariantCulture);
        }
    }
}