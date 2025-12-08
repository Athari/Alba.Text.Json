using System.Dynamic;

namespace Alba.Text.Json.Dynamic;

internal static partial class ExpressionExts
{
    extension<T>(T[] @this)
    {
        public T Single()
        {
            if (@this.Length != 1)
                throw new ArgumentException("Array must contain one element.", nameof(@this));
            return @this[0];
        }

        public TResult[] SelectArray<TResult>(Func<T, TResult> selector)
        {
            var ret = new TResult[@this.Length];
            for (var i = 0; i < @this.Length; i++)
                ret[i] = selector(@this[i]);
            return ret;
        }
    }

    extension(E @this)
    {
        public dobject ToDObject(object? value) =>
            dobject.Create(value!, @this);

        public dobject ToDObject(object? value, BindingRestrictions? r) =>
            new(@this, r ?? BindingRestrictions.Empty, value!);

        public dobject ToDObject(BindingRestrictions r) =>
            new(@this, r);

        public E EConvertIfNeeded(Type type) =>
            @this.Type == type ? @this : @this.EConvert(type);

        public E EConvertIfNeeded<T>() =>
            @this.EConvertIfNeeded(typeof(T));
    }

    extension(dobject @this)
    {
        public E TypedExpression =>
            @this.Expression.EConvertIfNeeded(@this.LimitType);

        public dobject Fallback(InvokeMemberBinder binder, dobject[] args) =>
            binder.FallbackInvokeMember(@this, args);

        public dobject Fallback(BinaryOperationBinder binder, dobject arg) =>
            binder.FallbackBinaryOperation(@this, arg);
    }

    extension(dobject[] objects)
    {
        public E[] SelectExpressions() => objects.SelectArray(o => o.Expression);
        public E[] SelectTypedExpressions() => objects.SelectArray(o => o.TypedExpression);
        public Type[] SelectTypes() => objects.SelectArray(o => o.LimitType);
        public Type[] SelectType(int i1) => [ objects[i1].LimitType ];
    }
}