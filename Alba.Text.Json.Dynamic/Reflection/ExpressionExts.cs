using System.Dynamic;
using CommunityToolkit.Diagnostics;

namespace Alba.Text.Json.Dynamic;

internal static partial class ExpressionExts
{
    public static T Single<T>(this T[] @this)
    {
        Guard.IsEqualTo(@this.Length, 1);
        return @this[0];
    }

    public static TResult[] SelectArray<T, TResult>(this T[] @this, Func<T, TResult> selector)
    {
        var ret = new TResult[@this.Length];
        for (var i = 0; i < @this.Length; i++)
            ret[i] = selector(@this[i]);
        return ret;
    }

    public static dobject ToDObject(this E @this, object value) => dobject.Create(value, @this);
    public static dobject ToDObject(this E @this, object value, BindingRestrictions? r) => new(@this, r ?? BindingRestrictions.Empty, value);
    public static dobject ToDObject(this E @this, BindingRestrictions r) => new(@this, r);

    public static E EConvertIfNeeded(this E @this, Type type) => @this.Type == type ? @this : @this.EConvert(type);
    public static E EConvertIfNeeded<T>(this E @this) => @this.EConvertIfNeeded(typeof(T));

    public static dobject Fallback(this dobject @this, InvokeMemberBinder binder, dobject[] args) =>
        binder.FallbackInvokeMember(@this, args);

    public static E[] SelectExpressions(this dobject[] objects) => objects.SelectArray(o => o.Expression);
    public static Type[] SelectTypes(this dobject[] objects) => objects.SelectArray(o => o.LimitType);
}