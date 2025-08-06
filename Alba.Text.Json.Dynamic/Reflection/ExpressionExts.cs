using System.Dynamic;

namespace Alba.Text.Json.Dynamic;

internal static partial class ExpressionExts
{
    public static T Single<T>(this T[] @this)
    {
        if (@this.Length != 1)
            throw new ArgumentException("Array must contain one element.", nameof(@this));
        return @this[0];
    }

    public static TResult[] SelectArray<T, TResult>(this T[] @this, Func<T, TResult> selector)
    {
        var ret = new TResult[@this.Length];
        for (var i = 0; i < @this.Length; i++)
            ret[i] = selector(@this[i]);
        return ret;
    }

    public static dobject ToDObject(this E @this, object? value) =>
        dobject.Create(value!, @this);

    public static dobject ToDObject(this E @this, object? value, BindingRestrictions? r) =>
        new(@this, r ?? BindingRestrictions.Empty, value!);

    public static dobject ToDObject(this E @this, BindingRestrictions r) =>
        new(@this, r);

    public static E EConvertIfNeeded(this E @this, Type type) =>
        @this.Type == type ? @this : @this.EConvert(type);

    public static E EConvertIfNeeded<T>(this E @this) =>
        @this.EConvertIfNeeded(typeof(T));

    public static dobject Fallback(this dobject @this, InvokeMemberBinder binder, dobject[] args) =>
        binder.FallbackInvokeMember(@this, args);

    public static dobject Fallback(this dobject @this, BinaryOperationBinder binder, dobject arg) =>
        binder.FallbackBinaryOperation(@this, arg);

    public static E GetTypedExpression(this dobject @this) =>
        @this.Expression.EConvertIfNeeded(@this.LimitType);

    public static E[] SelectExpressions(this dobject[] objects) => objects.SelectArray(o => o.Expression);
    public static E[] SelectTypedExpressions(this dobject[] objects) => objects.SelectArray(o => o.GetTypedExpression());
    public static Type[] SelectTypes(this dobject[] objects) => objects.SelectArray(o => o.LimitType);
    public static Type[] SelectType(this dobject[] objects, int i1) => [ objects[i1].LimitType ];
}