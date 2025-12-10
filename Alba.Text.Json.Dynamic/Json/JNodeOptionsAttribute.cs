using System.Text.Json.Serialization;

namespace Alba.Text.Json.Dynamic;

/// <summary>When placed on a property, field, or type, specifies the converter type to use. The target must be assignable <see cref="JNode"/>.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed partial class JNodeOptionsAttribute : JsonConverterAttribute
{
    /// <summary>Passes options from <see cref="JNodeOptionsAttribute"/> to <see cref="JNodeConverter"/>.</summary>
    /// <exception cref="ArgumentException">The target is not assignable to <see cref="JNode"/>.</exception>
    public override JsonConverter CreateConverter(Type type)
    {
        if (!type.IsAssignableTo(typeof(JNode)))
            throw new ArgumentException($"{nameof(JNodeOptionsAttribute)} can only be applied to {nameof(JNode)} properties and fields.");
        return new JNodeConverter(ToOptions());
    }
}