using System.Linq.Expressions;
using System.Reflection;

namespace Alba.Text.Json.Dynamic;

internal sealed class FieldRef
{
    public readonly Type Type;
    public readonly string Name;
    public readonly BindingFlags Flags;
    public readonly FieldInfo Field;

    private FieldRef(LambdaExpression expr)
    {
        var member = (MemberExpression)expr.Body;
        var field = (FieldInfo)member.Member;

        Type = field.DeclaringType!;
        Name = field.Name;
        Flags =
            (field.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic)
          | (field.IsStatic ? BindingFlags.Static : BindingFlags.Instance);
        Field = field;
    }

    public static FieldRef Of<TResult>(Expression<Func<TResult>> expr) => new(expr);
    public static FieldRef Of<T, TResult>(Expression<Func<T, TResult>> expr) => new(expr);
}