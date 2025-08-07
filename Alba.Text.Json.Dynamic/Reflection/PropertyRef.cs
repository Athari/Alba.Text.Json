using System.Linq.Expressions;
using System.Reflection;

namespace Alba.Text.Json.Dynamic;

internal sealed class PropertyRef
{
    public readonly Type Type;
    public readonly string Name;
    public readonly PropertyInfo Property;
    public readonly MethodRef Getter;
    public readonly MethodRef? Setter;

    private PropertyRef(LambdaExpression expr)
    {
        var member = (MemberExpression)expr.Body;
        var property = (PropertyInfo)member.Member;
        if (property.GetMethod == null)
            throw new ArgumentException($"Property {Name} has no getter.");

        Type = property.DeclaringType!;
        Name = property.Name;
        Property = property;

        Getter = new(property.GetMethod, [ member.Expression! ]);
        Setter = property.SetMethod != null
            ? new(property.SetMethod, [ member.Expression!, E.Default(property.PropertyType) ]) : null;
    }

    public static PropertyRef Of<TResult>(Expression<Func<TResult>> expr) => new(expr);
    public static PropertyRef Of<T, TResult>(Expression<Func<T, TResult>> expr) => new(expr);
}