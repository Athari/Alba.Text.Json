using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

public class JValue(JsonValue source, JNodeOptions? options = null)
    : JNode<JsonValue>(source, options), IDynamicMetaObjectProvider
{
    public dobject GetMetaObject(E expression) => new MetaJValue(expression, this);

    private class MetaJValue(E expression, JValue dynamicValue)
        : MetaJNode<JValue>(expression, dynamicValue)
    {
        public override dobject BindConvert(ConvertBinder binder)
        {
            return new(Expression.EConvertIfNeeded(binder.ReturnType), GetRestrictions());
        }
    }
}