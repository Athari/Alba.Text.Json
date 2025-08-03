#nullable disable

using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Alba.Text.Json.Dynamic;

internal static partial class ExpressionExts
{
    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ArrayIndex(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression EArrayIndex(this Expression array, params Expression[] indexes) =>
        Expression.ArrayIndex(array, indexes);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ArrayIndex(System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression EArrayIndex(this Expression array, IEnumerable<Expression> indexes) =>
        Expression.ArrayIndex(array, indexes);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ArrayIndex(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BinaryExpression EArrayIndex(this Expression array, Expression index) =>
        Expression.ArrayIndex(array, index);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression,System.Type)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnaryExpression EConvert(this Expression expression, Type type) =>
        Expression.Convert(expression, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression,System.Type)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnaryExpression EConvert<T>(this Expression expression) =>
        Expression.Convert(expression, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression,System.Type,System.Reflection.MethodInfo)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnaryExpression EConvert(this Expression expression, Type type, MethodInfo method) =>
        Expression.Convert(expression, type, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression,System.Type,System.Reflection.MethodInfo)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnaryExpression EConvert<T>(this Expression expression, MethodInfo method) =>
        Expression.Convert(expression, typeof(T), method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression,System.Reflection.FieldInfo)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberExpression EField(this Expression expression, FieldInfo field) =>
        Expression.Field(expression, field);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression,System.String)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberExpression EField(this Expression expression, string fieldName) =>
        Expression.Field(expression, fieldName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression,System.Type,System.String)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberExpression EField(this Expression expression, Type type, string fieldName) =>
        Expression.Field(expression, type, fieldName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression,System.Type,System.String)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberExpression EField<T>(this Expression expression, string fieldName) =>
        Expression.Field(expression, typeof(T), fieldName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnaryExpression ENot(this Expression expression) =>
        Expression.Not(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnaryExpression ENot(this Expression expression, MethodInfo method) =>
        Expression.Not(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.String)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberExpression EProperty(this Expression expression, string propertyName) =>
        Expression.Property(expression, propertyName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Type,System.String)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberExpression EProperty(this Expression expression, Type type, string propertyName) =>
        Expression.Property(expression, type, propertyName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Type,System.String)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberExpression EProperty<T>(this Expression expression, string propertyName) =>
        Expression.Property(expression, typeof(T), propertyName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Reflection.PropertyInfo)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberExpression EProperty(this Expression expression, PropertyInfo property) =>
        Expression.Property(expression, property);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberExpression EProperty(this Expression expression, MethodInfo propertyAccessor) =>
        Expression.Property(expression, propertyAccessor);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method) =>
        Expression.Call(instance, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.Expression[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method, params Expression[] arguments) =>
        Expression.Call(instance, method, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method, Expression arg0, Expression arg1) =>
        Expression.Call(instance, method, arg0, arg1);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method, Expression arg0, Expression arg1, Expression arg2) =>
        Expression.Call(instance, method, arg0, arg1, arg2);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.String,System.Type[],System.Linq.Expressions.Expression[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression ECall(this Expression instance, string methodName, Type[] typeArguments, params Expression[] arguments) =>
        Expression.Call(instance, methodName, typeArguments, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method, IEnumerable<Expression> arguments) =>
        Expression.Call(instance, method, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.String,System.Linq.Expressions.Expression[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IndexExpression EProperty(this Expression instance, string propertyName, params Expression[] arguments) =>
        Expression.Property(instance, propertyName, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Reflection.PropertyInfo,System.Linq.Expressions.Expression[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IndexExpression EProperty(this Expression instance, PropertyInfo indexer, params Expression[] arguments) =>
        Expression.Property(instance, indexer, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Reflection.PropertyInfo,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IndexExpression EProperty(this Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments) =>
        Expression.Property(instance, indexer, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BinaryExpression EAssign(this Expression left, Expression right) =>
        Expression.Assign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Type,System.String,System.Type[],System.Linq.Expressions.Expression[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression ECall(this Type type, string methodName, Type[] typeArguments, params Expression[] arguments) =>
        Expression.Call(type, methodName, typeArguments, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Type,System.String,System.Type[],System.Linq.Expressions.Expression[])" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MethodCallExpression ECall<T>(this string methodName, Type[] typeArguments, params Expression[] arguments) =>
        Expression.Call(typeof(T), methodName, typeArguments, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Constant(System.Object)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConstantExpression EConst(this object value) =>
        Expression.Constant(value);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Constant(System.Object,System.Type)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConstantExpression EConst(this object value, Type type) =>
        Expression.Constant(value, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Constant(System.Object,System.Type)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConstantExpression EConst<T>(this object value) =>
        Expression.Constant(value, typeof(T));

}