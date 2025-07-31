using System.Text.Json.Serialization;

namespace Alba.Text.Json.Dynamic;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public partial class JNodeOptionsAttribute : JsonConverterAttribute
{
    public override JsonConverter CreateConverter(Type type)
    {
        if (!type.IsAssignableTo(typeof(JNode)))
            throw new ArgumentException($"{nameof(JNodeOptionsAttribute)} can only be applied to {nameof(JNode)} properties and fields.");
        return new JNodeConverter(new(this));
    }
}