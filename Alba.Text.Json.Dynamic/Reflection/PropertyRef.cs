using System.Linq.Expressions;
using System.Reflection;

namespace Alba.Text.Json.Dynamic;

internal sealed class PropertyRef
{
    public readonly Type Type;
    public readonly string Name;
    //public readonly BindingFlags Flags;
    public readonly PropertyInfo Property;
    public readonly MethodInfo Getter;
    public readonly MethodInfo? Setter;

    private PropertyRef(LambdaExpression expr)
    {
        var member = (MemberExpression)expr.Body;
        var property = (PropertyInfo)member.Member;

        Type = property.DeclaringType!;
        Name = property.Name;
        //Flags =
        //    (property.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic)
        //  | (property.IsStatic ? BindingFlags.Static : BindingFlags.Instance);
        Property = property;
        Getter = property.GetMethod ?? throw new ArgumentException($"Property {Name} has no getter.");
        Setter = property.SetMethod;
    }

    public static PropertyRef Of<TResult>(Expression<Func<TResult>> expr) => new(expr);
    public static PropertyRef Of<T, TResult>(Expression<Func<T, TResult>> expr) => new(expr);
}