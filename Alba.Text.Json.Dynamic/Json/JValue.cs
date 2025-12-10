using System.Dynamic;
using System.Text.Json.Nodes;

namespace Alba.Text.Json.Dynamic;

/// <summary>Dynamic adapter for <see cref="JsonValue"/> with support for dynamic dispatch via <see cref="IDynamicMetaObjectProvider"/>.</summary>
/// <param name="source">Source <see cref="JsonValue"/>.</param>
/// <param name="options">Options to control the behavior.</param>
/// <remarks>Currently not used as values are exposed directly.</remarks>
internal sealed class JValue(JsonValue source, JNodeOptions? options = null)
    : JNode<JsonValue>(source, options), IDynamicMetaObjectProvider
{
    /// <summary>Creates a new instance of the <see cref="JValue"/> with the value of the current node.</summary>
    /// <returns>A new cloned instance of the current node.</returns>
    public new JValue Clone() =>
        (JValue)base.Clone();

    dobject IDynamicMetaObjectProvider.GetMetaObject(E expression) =>
        new MetaJValue(expression, this);

    private sealed class MetaJValue(E expression, JValue dynamicValue)
        : MetaJNode<JValue>(expression, dynamicValue)
    {
        public override dobject BindConvert(ConvertBinder binder)
        {
            return new(Expression.EConvertIfNeeded(binder.ReturnType), GetRestrictions());
        }
    }
}