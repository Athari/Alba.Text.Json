using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public sealed class JObject(JsonObject source, JNodeOptions? options = null)
    : JNode<JsonObject>(source, options), IDynamicMetaObjectProvider
{
    public dobject GetMetaObject(E expression) => new MetaJObject(expression, this);

    private class MetaJObject(E expression, JObject dynamicValue)
        : MetaJNode<JObject>(expression, dynamicValue)
    {
        public override dobject BindGetIndex(GetIndexBinder binder, dobject[] indexes) =>
            CallGetMember(indexes.Single().Expression);

        public override dobject BindSetIndex(SetIndexBinder binder, dobject[] indexes, dobject value) =>
            CallSetMember(indexes.Single().Expression, value);

        public override dobject BindGetMember(GetMemberBinder binder) =>
            CallGetMember(binder.Name.EConst());

        public override dobject BindSetMember(SetMemberBinder binder, dobject value) =>
            CallSetMember(binder.Name.EConst(), value);

        private dobject CallGetMember(E arg) =>
            CallStaticMethod(JOperations.JsonObject_GetMember_Method,
                [ ExprSelf(), arg ]);

        private dobject CallSetMember(E arg, dobject value) =>
            CallStaticMethod(JOperations.JsonObject_SetMember_Method,
                [ ExprSelf(), arg, value.Expression ], [ value.LimitType ]);

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach ((string key, JsonNode? _) in Value.Node)
                yield return key;
        }
    }
}