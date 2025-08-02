using System.Dynamic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Alba.Text.Json.Dynamic;

[JsonConverter(typeof(JNodeConverter))]
public abstract partial class JNode(JNodeOptions? options = null)
{
    internal readonly JNodeOptions Options = options ?? JNodeOptions.Default;

    internal abstract JsonNode NodeUntyped { get; }

    private protected abstract class MetaJsonNodeBase(E expression, JNode value)
        : dobject(expression, BindingRestrictions.Empty, value)
    {
        protected BindingRestrictions GetRestrictions() =>
            BindingRestrictions.GetTypeRestriction(Expression, LimitType);

        protected dobject CallStaticMethod(MethodRef m, E[] parameters, Type[]? genericTypes = null) =>
            new(E.Call(null, m.GetMethod(genericTypes), parameters), GetRestrictions());
    }
}