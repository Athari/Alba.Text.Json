#nullable enable

using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Alba.Text.Json.Dynamic;

internal static partial class ExpressionExts
{
    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BlockExpression EBlock(this Expression arg0, Expression arg1) =>
        Expression.Block(arg0, arg1);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BlockExpression EBlock(this Expression arg0, Expression arg1, Expression arg2) =>
        Expression.Block(arg0, arg1, arg2);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BlockExpression EBlock(this Expression arg0, Expression arg1, Expression arg2, Expression arg3) =>
        Expression.Block(arg0, arg1, arg2, arg3);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BlockExpression EBlock(this Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4) =>
        Expression.Block(arg0, arg1, arg2, arg3, arg4);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ArrayAccess(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression[])" />
    public static IndexExpression EArrayAccess(this Expression array, params Expression[] indexes) =>
        Expression.ArrayAccess(array, indexes);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ArrayAccess(System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static IndexExpression EArrayAccess(this Expression array, IEnumerable<Expression> indexes) =>
        Expression.ArrayAccess(array, indexes);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ArrayIndex(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression[])" />
    public static MethodCallExpression EArrayIndex(this Expression array, params Expression[] indexes) =>
        Expression.ArrayIndex(array, indexes);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ArrayIndex(System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static MethodCallExpression EArrayIndex(this Expression array, IEnumerable<Expression> indexes) =>
        Expression.ArrayIndex(array, indexes);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ArrayIndex(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EArrayIndex(this Expression array, Expression index) =>
        Expression.ArrayIndex(array, index);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ArrayLength(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EArrayLength(this Expression array) =>
        Expression.ArrayLength(array);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda``1(System.Linq.Expressions.Expression,System.Linq.Expressions.ParameterExpression[])" />
    public static Expression<TDelegate> ELambda<TDelegate>(this Expression body, params ParameterExpression[] parameters) =>
        Expression.Lambda<TDelegate>(body, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda``1(System.Linq.Expressions.Expression,System.Boolean,System.Linq.Expressions.ParameterExpression[])" />
    public static Expression<TDelegate> ELambda<TDelegate>(this Expression body, Boolean tailCall, params ParameterExpression[] parameters) =>
        Expression.Lambda<TDelegate>(body, tailCall, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda``1(System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static Expression<TDelegate> ELambda<TDelegate>(this Expression body, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda<TDelegate>(body, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda``1(System.Linq.Expressions.Expression,System.Boolean,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static Expression<TDelegate> ELambda<TDelegate>(this Expression body, Boolean tailCall, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda<TDelegate>(body, tailCall, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda``1(System.Linq.Expressions.Expression,System.String,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static Expression<TDelegate> ELambda<TDelegate>(this Expression body, string name, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda<TDelegate>(body, name, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda``1(System.Linq.Expressions.Expression,System.String,System.Boolean,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static Expression<TDelegate> ELambda<TDelegate>(this Expression body, string name, Boolean tailCall, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda<TDelegate>(body, name, tailCall, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression,System.Linq.Expressions.ParameterExpression[])" />
    public static LambdaExpression ELambda(this Expression body, params ParameterExpression[] parameters) =>
        Expression.Lambda(body, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression,System.Boolean,System.Linq.Expressions.ParameterExpression[])" />
    public static LambdaExpression ELambda(this Expression body, Boolean tailCall, params ParameterExpression[] parameters) =>
        Expression.Lambda(body, tailCall, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static LambdaExpression ELambda(this Expression body, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda(body, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression,System.Boolean,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static LambdaExpression ELambda(this Expression body, Boolean tailCall, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda(body, tailCall, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression,System.String,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static LambdaExpression ELambda(this Expression body, string name, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda(body, name, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Linq.Expressions.Expression,System.String,System.Boolean,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static LambdaExpression ELambda(this Expression body, string name, Boolean tailCall, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda(body, name, tailCall, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Loop(System.Linq.Expressions.Expression)" />
    public static LoopExpression ELoop(this Expression body) =>
        Expression.Loop(body);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Loop(System.Linq.Expressions.Expression,System.Linq.Expressions.LabelTarget)" />
    public static LoopExpression ELoop(this Expression body, LabelTarget @break) =>
        Expression.Loop(body, @break);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Loop(System.Linq.Expressions.Expression,System.Linq.Expressions.LabelTarget,System.Linq.Expressions.LabelTarget)" />
    public static LoopExpression ELoop(this Expression body, LabelTarget @break, LabelTarget @continue) =>
        Expression.Loop(body, @break, @continue);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SwitchCase(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression[])" />
    public static SwitchCase ESwitchCase(this Expression body, params Expression[] testValues) =>
        Expression.SwitchCase(body, testValues);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SwitchCase(System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static SwitchCase ESwitchCase(this Expression body, IEnumerable<Expression> testValues) =>
        Expression.SwitchCase(body, testValues);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TryCatch(System.Linq.Expressions.Expression,System.Linq.Expressions.CatchBlock[])" />
    public static TryExpression ETryCatch(this Expression body, params CatchBlock[] handlers) =>
        Expression.TryCatch(body, handlers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TryCatchFinally(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.CatchBlock[])" />
    public static TryExpression ETryCatchFinally(this Expression body, Expression @finally, params CatchBlock[] handlers) =>
        Expression.TryCatchFinally(body, @finally, handlers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TryFault(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static TryExpression ETryFault(this Expression body, Expression fault) =>
        Expression.TryFault(body, fault);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TryFinally(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static TryExpression ETryFinally(this Expression body, Expression @finally) =>
        Expression.TryFinally(body, @finally);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EConvert(this Expression expression, Type type) =>
        Expression.Convert(expression, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EConvert<T>(this Expression expression) =>
        Expression.Convert(expression, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression,System.Type,System.Reflection.MethodInfo)" />
    public static UnaryExpression EConvert(this Expression expression, Type type, MethodInfo method) =>
        Expression.Convert(expression, type, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression,System.Type,System.Reflection.MethodInfo)" />
    public static UnaryExpression EConvert<T>(this Expression expression, MethodInfo method) =>
        Expression.Convert(expression, typeof(T), method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ConvertChecked(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EConvertChecked(this Expression expression, Type type) =>
        Expression.ConvertChecked(expression, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ConvertChecked(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EConvertChecked<T>(this Expression expression) =>
        Expression.ConvertChecked(expression, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ConvertChecked(System.Linq.Expressions.Expression,System.Type,System.Reflection.MethodInfo)" />
    public static UnaryExpression EConvertChecked(this Expression expression, Type type, MethodInfo method) =>
        Expression.ConvertChecked(expression, type, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ConvertChecked(System.Linq.Expressions.Expression,System.Type,System.Reflection.MethodInfo)" />
    public static UnaryExpression EConvertChecked<T>(this Expression expression, MethodInfo method) =>
        Expression.ConvertChecked(expression, typeof(T), method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Decrement(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EDecrement(this Expression expression) =>
        Expression.Decrement(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Decrement(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EDecrement(this Expression expression, MethodInfo method) =>
        Expression.Decrement(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression,System.Reflection.FieldInfo)" />
    public static MemberExpression EField(this Expression expression, FieldInfo field) =>
        Expression.Field(expression, field);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression,System.String)" />
    public static MemberExpression EField(this Expression expression, string fieldName) =>
        Expression.Field(expression, fieldName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression,System.Type,System.String)" />
    public static MemberExpression EField(this Expression expression, Type type, string fieldName) =>
        Expression.Field(expression, type, fieldName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression,System.Type,System.String)" />
    public static MemberExpression EField<T>(this Expression expression, string fieldName) =>
        Expression.Field(expression, typeof(T), fieldName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Increment(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EIncrement(this Expression expression) =>
        Expression.Increment(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Increment(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EIncrement(this Expression expression, MethodInfo method) =>
        Expression.Increment(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Invoke(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression[])" />
    public static InvocationExpression EInvoke(this Expression expression, params Expression[] arguments) =>
        Expression.Invoke(expression, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Invoke(System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static InvocationExpression EInvoke(this Expression expression, IEnumerable<Expression> arguments) =>
        Expression.Invoke(expression, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.IsFalse(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EIsFalse(this Expression expression) =>
        Expression.IsFalse(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.IsFalse(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EIsFalse(this Expression expression, MethodInfo method) =>
        Expression.IsFalse(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.IsTrue(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EIsTrue(this Expression expression) =>
        Expression.IsTrue(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.IsTrue(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EIsTrue(this Expression expression, MethodInfo method) =>
        Expression.IsTrue(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeMemberAccess(System.Linq.Expressions.Expression,System.Reflection.MemberInfo)" />
    public static MemberExpression EMakeMemberAccess(this Expression expression, MemberInfo member) =>
        Expression.MakeMemberAccess(expression, member);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Negate(System.Linq.Expressions.Expression)" />
    public static UnaryExpression ENegate(this Expression expression) =>
        Expression.Negate(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Negate(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression ENegate(this Expression expression, MethodInfo method) =>
        Expression.Negate(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NegateChecked(System.Linq.Expressions.Expression)" />
    public static UnaryExpression ENegateChecked(this Expression expression) =>
        Expression.NegateChecked(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NegateChecked(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression ENegateChecked(this Expression expression, MethodInfo method) =>
        Expression.NegateChecked(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression)" />
    public static UnaryExpression ENot(this Expression expression) =>
        Expression.Not(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression ENot(this Expression expression, MethodInfo method) =>
        Expression.Not(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.OnesComplement(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EOnesComplement(this Expression expression) =>
        Expression.OnesComplement(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.OnesComplement(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EOnesComplement(this Expression expression, MethodInfo method) =>
        Expression.OnesComplement(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PostDecrementAssign(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EPostDecrementAssign(this Expression expression) =>
        Expression.PostDecrementAssign(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PostDecrementAssign(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EPostDecrementAssign(this Expression expression, MethodInfo method) =>
        Expression.PostDecrementAssign(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PostIncrementAssign(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EPostIncrementAssign(this Expression expression) =>
        Expression.PostIncrementAssign(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PostIncrementAssign(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EPostIncrementAssign(this Expression expression, MethodInfo method) =>
        Expression.PostIncrementAssign(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PreDecrementAssign(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EPreDecrementAssign(this Expression expression) =>
        Expression.PreDecrementAssign(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PreDecrementAssign(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EPreDecrementAssign(this Expression expression, MethodInfo method) =>
        Expression.PreDecrementAssign(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PreIncrementAssign(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EPreIncrementAssign(this Expression expression) =>
        Expression.PreIncrementAssign(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PreIncrementAssign(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EPreIncrementAssign(this Expression expression, MethodInfo method) =>
        Expression.PreIncrementAssign(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.String)" />
    public static MemberExpression EProperty(this Expression expression, string propertyName) =>
        Expression.Property(expression, propertyName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Type,System.String)" />
    public static MemberExpression EProperty(this Expression expression, Type type, string propertyName) =>
        Expression.Property(expression, type, propertyName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Type,System.String)" />
    public static MemberExpression EProperty<T>(this Expression expression, string propertyName) =>
        Expression.Property(expression, typeof(T), propertyName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Reflection.PropertyInfo)" />
    public static MemberExpression EProperty(this Expression expression, PropertyInfo property) =>
        Expression.Property(expression, property);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static MemberExpression EProperty(this Expression expression, MethodInfo propertyAccessor) =>
        Expression.Property(expression, propertyAccessor);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PropertyOrField(System.Linq.Expressions.Expression,System.String)" />
    public static MemberExpression EPropertyOrField(this Expression expression, string propertyOrFieldName) =>
        Expression.PropertyOrField(expression, propertyOrFieldName);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Quote(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EQuote(this Expression expression) =>
        Expression.Quote(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TypeAs(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression ETypeAs(this Expression expression, Type type) =>
        Expression.TypeAs(expression, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TypeAs(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression ETypeAs<T>(this Expression expression) =>
        Expression.TypeAs(expression, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TypeEqual(System.Linq.Expressions.Expression,System.Type)" />
    public static TypeBinaryExpression ETypeEqual(this Expression expression, Type type) =>
        Expression.TypeEqual(expression, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TypeEqual(System.Linq.Expressions.Expression,System.Type)" />
    public static TypeBinaryExpression ETypeEqual<T>(this Expression expression) =>
        Expression.TypeEqual(expression, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TypeIs(System.Linq.Expressions.Expression,System.Type)" />
    public static TypeBinaryExpression ETypeIs(this Expression expression, Type type) =>
        Expression.TypeIs(expression, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.TypeIs(System.Linq.Expressions.Expression,System.Type)" />
    public static TypeBinaryExpression ETypeIs<T>(this Expression expression) =>
        Expression.TypeIs(expression, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.UnaryPlus(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EUnaryPlus(this Expression expression) =>
        Expression.UnaryPlus(expression);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.UnaryPlus(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static UnaryExpression EUnaryPlus(this Expression expression, MethodInfo method) =>
        Expression.UnaryPlus(expression, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Unbox(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EUnbox(this Expression expression, Type type) =>
        Expression.Unbox(expression, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Unbox(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EUnbox<T>(this Expression expression) =>
        Expression.Unbox(expression, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method) =>
        Expression.Call(instance, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.Expression[])" />
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method, params Expression[] arguments) =>
        Expression.Call(instance, method, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method, Expression arg0, Expression arg1) =>
        Expression.Call(instance, method, arg0, arg1);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method, Expression arg0, Expression arg1, Expression arg2) =>
        Expression.Call(instance, method, arg0, arg1, arg2);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.String,System.Type[],System.Linq.Expressions.Expression[])" />
    public static MethodCallExpression ECall(this Expression instance, string methodName, Type[] typeArguments, params Expression[] arguments) =>
        Expression.Call(instance, methodName, typeArguments, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static MethodCallExpression ECall(this Expression instance, MethodInfo method, IEnumerable<Expression> arguments) =>
        Expression.Call(instance, method, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeIndex(System.Linq.Expressions.Expression,System.Reflection.PropertyInfo,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static IndexExpression EMakeIndex(this Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments) =>
        Expression.MakeIndex(instance, indexer, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.String,System.Linq.Expressions.Expression[])" />
    public static IndexExpression EProperty(this Expression instance, string propertyName, params Expression[] arguments) =>
        Expression.Property(instance, propertyName, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Reflection.PropertyInfo,System.Linq.Expressions.Expression[])" />
    public static IndexExpression EProperty(this Expression instance, PropertyInfo indexer, params Expression[] arguments) =>
        Expression.Property(instance, indexer, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression,System.Reflection.PropertyInfo,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static IndexExpression EProperty(this Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments) =>
        Expression.Property(instance, indexer, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Add(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EAdd(this Expression left, Expression right) =>
        Expression.Add(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Add(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EAdd(this Expression left, Expression right, MethodInfo method) =>
        Expression.Add(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AddAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EAddAssign(this Expression left, Expression right) =>
        Expression.AddAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AddAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EAddAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.AddAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AddAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EAddAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.AddAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AddAssignChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EAddAssignChecked(this Expression left, Expression right) =>
        Expression.AddAssignChecked(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AddAssignChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EAddAssignChecked(this Expression left, Expression right, MethodInfo method) =>
        Expression.AddAssignChecked(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AddAssignChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EAddAssignChecked(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.AddAssignChecked(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AddChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EAddChecked(this Expression left, Expression right) =>
        Expression.AddChecked(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AddChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EAddChecked(this Expression left, Expression right, MethodInfo method) =>
        Expression.AddChecked(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.And(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EAnd(this Expression left, Expression right) =>
        Expression.And(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.And(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EAnd(this Expression left, Expression right, MethodInfo method) =>
        Expression.And(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AndAlso(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EAndAlso(this Expression left, Expression right) =>
        Expression.AndAlso(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AndAlso(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EAndAlso(this Expression left, Expression right, MethodInfo method) =>
        Expression.AndAlso(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AndAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EAndAssign(this Expression left, Expression right) =>
        Expression.AndAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AndAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EAndAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.AndAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.AndAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EAndAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.AndAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EAssign(this Expression left, Expression right) =>
        Expression.Assign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Coalesce(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ECoalesce(this Expression left, Expression right) =>
        Expression.Coalesce(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Coalesce(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression ECoalesce(this Expression left, Expression right, LambdaExpression conversion) =>
        Expression.Coalesce(left, right, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Divide(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EDivide(this Expression left, Expression right) =>
        Expression.Divide(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Divide(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EDivide(this Expression left, Expression right, MethodInfo method) =>
        Expression.Divide(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.DivideAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EDivideAssign(this Expression left, Expression right) =>
        Expression.DivideAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.DivideAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EDivideAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.DivideAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.DivideAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EDivideAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.DivideAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Equal(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EEqual(this Expression left, Expression right) =>
        Expression.Equal(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Equal(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Boolean,System.Reflection.MethodInfo)" />
    public static BinaryExpression EEqual(this Expression left, Expression right, Boolean liftToNull, MethodInfo method) =>
        Expression.Equal(left, right, liftToNull, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ExclusiveOr(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EExclusiveOr(this Expression left, Expression right) =>
        Expression.ExclusiveOr(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ExclusiveOr(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EExclusiveOr(this Expression left, Expression right, MethodInfo method) =>
        Expression.ExclusiveOr(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ExclusiveOrAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EExclusiveOrAssign(this Expression left, Expression right) =>
        Expression.ExclusiveOrAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ExclusiveOrAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EExclusiveOrAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.ExclusiveOrAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ExclusiveOrAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EExclusiveOrAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.ExclusiveOrAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.GreaterThan(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EGreaterThan(this Expression left, Expression right) =>
        Expression.GreaterThan(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.GreaterThan(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Boolean,System.Reflection.MethodInfo)" />
    public static BinaryExpression EGreaterThan(this Expression left, Expression right, Boolean liftToNull, MethodInfo method) =>
        Expression.GreaterThan(left, right, liftToNull, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.GreaterThanOrEqual(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EGreaterThanOrEqual(this Expression left, Expression right) =>
        Expression.GreaterThanOrEqual(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.GreaterThanOrEqual(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Boolean,System.Reflection.MethodInfo)" />
    public static BinaryExpression EGreaterThanOrEqual(this Expression left, Expression right, Boolean liftToNull, MethodInfo method) =>
        Expression.GreaterThanOrEqual(left, right, liftToNull, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.LeftShift(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ELeftShift(this Expression left, Expression right) =>
        Expression.LeftShift(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.LeftShift(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression ELeftShift(this Expression left, Expression right, MethodInfo method) =>
        Expression.LeftShift(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.LeftShiftAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ELeftShiftAssign(this Expression left, Expression right) =>
        Expression.LeftShiftAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.LeftShiftAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression ELeftShiftAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.LeftShiftAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.LeftShiftAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression ELeftShiftAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.LeftShiftAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.LessThan(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ELessThan(this Expression left, Expression right) =>
        Expression.LessThan(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.LessThan(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Boolean,System.Reflection.MethodInfo)" />
    public static BinaryExpression ELessThan(this Expression left, Expression right, Boolean liftToNull, MethodInfo method) =>
        Expression.LessThan(left, right, liftToNull, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.LessThanOrEqual(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ELessThanOrEqual(this Expression left, Expression right) =>
        Expression.LessThanOrEqual(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.LessThanOrEqual(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Boolean,System.Reflection.MethodInfo)" />
    public static BinaryExpression ELessThanOrEqual(this Expression left, Expression right, Boolean liftToNull, MethodInfo method) =>
        Expression.LessThanOrEqual(left, right, liftToNull, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Modulo(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EModulo(this Expression left, Expression right) =>
        Expression.Modulo(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Modulo(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EModulo(this Expression left, Expression right, MethodInfo method) =>
        Expression.Modulo(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ModuloAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EModuloAssign(this Expression left, Expression right) =>
        Expression.ModuloAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ModuloAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EModuloAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.ModuloAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ModuloAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EModuloAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.ModuloAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Multiply(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EMultiply(this Expression left, Expression right) =>
        Expression.Multiply(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Multiply(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EMultiply(this Expression left, Expression right, MethodInfo method) =>
        Expression.Multiply(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MultiplyAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EMultiplyAssign(this Expression left, Expression right) =>
        Expression.MultiplyAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MultiplyAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EMultiplyAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.MultiplyAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MultiplyAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EMultiplyAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.MultiplyAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MultiplyAssignChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EMultiplyAssignChecked(this Expression left, Expression right) =>
        Expression.MultiplyAssignChecked(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MultiplyAssignChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EMultiplyAssignChecked(this Expression left, Expression right, MethodInfo method) =>
        Expression.MultiplyAssignChecked(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MultiplyAssignChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EMultiplyAssignChecked(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.MultiplyAssignChecked(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MultiplyChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EMultiplyChecked(this Expression left, Expression right) =>
        Expression.MultiplyChecked(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MultiplyChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EMultiplyChecked(this Expression left, Expression right, MethodInfo method) =>
        Expression.MultiplyChecked(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NotEqual(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ENotEqual(this Expression left, Expression right) =>
        Expression.NotEqual(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NotEqual(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Boolean,System.Reflection.MethodInfo)" />
    public static BinaryExpression ENotEqual(this Expression left, Expression right, Boolean liftToNull, MethodInfo method) =>
        Expression.NotEqual(left, right, liftToNull, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Or(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EOr(this Expression left, Expression right) =>
        Expression.Or(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Or(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EOr(this Expression left, Expression right, MethodInfo method) =>
        Expression.Or(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.OrAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EOrAssign(this Expression left, Expression right) =>
        Expression.OrAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.OrAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EOrAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.OrAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.OrAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EOrAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.OrAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.OrElse(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EOrElse(this Expression left, Expression right) =>
        Expression.OrElse(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.OrElse(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EOrElse(this Expression left, Expression right, MethodInfo method) =>
        Expression.OrElse(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Power(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EPower(this Expression left, Expression right) =>
        Expression.Power(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Power(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EPower(this Expression left, Expression right, MethodInfo method) =>
        Expression.Power(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PowerAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EPowerAssign(this Expression left, Expression right) =>
        Expression.PowerAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PowerAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression EPowerAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.PowerAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.PowerAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EPowerAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.PowerAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ReferenceEqual(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EReferenceEqual(this Expression left, Expression right) =>
        Expression.ReferenceEqual(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ReferenceNotEqual(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EReferenceNotEqual(this Expression left, Expression right) =>
        Expression.ReferenceNotEqual(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.RightShift(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ERightShift(this Expression left, Expression right) =>
        Expression.RightShift(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.RightShift(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression ERightShift(this Expression left, Expression right, MethodInfo method) =>
        Expression.RightShift(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.RightShiftAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ERightShiftAssign(this Expression left, Expression right) =>
        Expression.RightShiftAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.RightShiftAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression ERightShiftAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.RightShiftAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.RightShiftAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression ERightShiftAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.RightShiftAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Subtract(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ESubtract(this Expression left, Expression right) =>
        Expression.Subtract(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Subtract(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression ESubtract(this Expression left, Expression right, MethodInfo method) =>
        Expression.Subtract(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SubtractAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ESubtractAssign(this Expression left, Expression right) =>
        Expression.SubtractAssign(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SubtractAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression ESubtractAssign(this Expression left, Expression right, MethodInfo method) =>
        Expression.SubtractAssign(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SubtractAssign(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression ESubtractAssign(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.SubtractAssign(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SubtractAssignChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ESubtractAssignChecked(this Expression left, Expression right) =>
        Expression.SubtractAssignChecked(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SubtractAssignChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression ESubtractAssignChecked(this Expression left, Expression right, MethodInfo method) =>
        Expression.SubtractAssignChecked(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SubtractAssignChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression ESubtractAssignChecked(this Expression left, Expression right, MethodInfo method, LambdaExpression conversion) =>
        Expression.SubtractAssignChecked(left, right, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SubtractChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression ESubtractChecked(this Expression left, Expression right) =>
        Expression.SubtractChecked(left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.SubtractChecked(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo)" />
    public static BinaryExpression ESubtractChecked(this Expression left, Expression right, MethodInfo method) =>
        Expression.SubtractChecked(left, right, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ListInit(System.Linq.Expressions.NewExpression,System.Linq.Expressions.ElementInit[])" />
    public static ListInitExpression EListInit(this NewExpression newExpression, params ElementInit[] initializers) =>
        Expression.ListInit(newExpression, initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ListInit(System.Linq.Expressions.NewExpression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ElementInit})" />
    public static ListInitExpression EListInit(this NewExpression newExpression, IEnumerable<ElementInit> initializers) =>
        Expression.ListInit(newExpression, initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ListInit(System.Linq.Expressions.NewExpression,System.Linq.Expressions.Expression[])" />
    public static ListInitExpression EListInit(this NewExpression newExpression, params Expression[] initializers) =>
        Expression.ListInit(newExpression, initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ListInit(System.Linq.Expressions.NewExpression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static ListInitExpression EListInit(this NewExpression newExpression, IEnumerable<Expression> initializers) =>
        Expression.ListInit(newExpression, initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ListInit(System.Linq.Expressions.NewExpression,System.Reflection.MethodInfo,System.Linq.Expressions.Expression[])" />
    public static ListInitExpression EListInit(this NewExpression newExpression, MethodInfo addMethod, params Expression[] initializers) =>
        Expression.ListInit(newExpression, addMethod, initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.ListInit(System.Linq.Expressions.NewExpression,System.Reflection.MethodInfo,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static ListInitExpression EListInit(this NewExpression newExpression, MethodInfo addMethod, IEnumerable<Expression> initializers) =>
        Expression.ListInit(newExpression, addMethod, initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MemberInit(System.Linq.Expressions.NewExpression,System.Linq.Expressions.MemberBinding[])" />
    public static MemberInitExpression EMemberInit(this NewExpression newExpression, params MemberBinding[] bindings) =>
        Expression.MemberInit(newExpression, bindings);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MemberInit(System.Linq.Expressions.NewExpression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.MemberBinding})" />
    public static MemberInitExpression EMemberInit(this NewExpression newExpression, IEnumerable<MemberBinding> bindings) =>
        Expression.MemberInit(newExpression, bindings);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Switch(System.Linq.Expressions.Expression,System.Linq.Expressions.SwitchCase[])" />
    public static SwitchExpression ESwitch(this Expression switchValue, params SwitchCase[] cases) =>
        Expression.Switch(switchValue, cases);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Switch(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.SwitchCase[])" />
    public static SwitchExpression ESwitch(this Expression switchValue, Expression defaultBody, params SwitchCase[] cases) =>
        Expression.Switch(switchValue, defaultBody, cases);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Switch(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.SwitchCase[])" />
    public static SwitchExpression ESwitch(this Expression switchValue, Expression defaultBody, MethodInfo comparison, params SwitchCase[] cases) =>
        Expression.Switch(switchValue, defaultBody, comparison, cases);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Switch(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Collections.Generic.IEnumerable{System.Linq.Expressions.SwitchCase})" />
    public static SwitchExpression ESwitch(this Expression switchValue, Expression defaultBody, MethodInfo comparison, IEnumerable<SwitchCase> cases) =>
        Expression.Switch(switchValue, defaultBody, comparison, cases);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static ConditionalExpression ECondition(this Expression test, Expression ifTrue, Expression ifFalse) =>
        Expression.Condition(test, ifTrue, ifFalse);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Type)" />
    public static ConditionalExpression ECondition(this Expression test, Expression ifTrue, Expression ifFalse, Type type) =>
        Expression.Condition(test, ifTrue, ifFalse, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Type)" />
    public static ConditionalExpression ECondition<T>(this Expression test, Expression ifTrue, Expression ifFalse) =>
        Expression.Condition(test, ifTrue, ifFalse, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.IfThen(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static ConditionalExpression EIfThen(this Expression test, Expression ifTrue) =>
        Expression.IfThen(test, ifTrue);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.IfThenElse(System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static ConditionalExpression EIfThenElse(this Expression test, Expression ifTrue, Expression ifFalse) =>
        Expression.IfThenElse(test, ifTrue, ifFalse);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Throw(System.Linq.Expressions.Expression)" />
    public static UnaryExpression EThrow(this Expression value) =>
        Expression.Throw(value);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Throw(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EThrow(this Expression value, Type type) =>
        Expression.Throw(value, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Throw(System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EThrow<T>(this Expression value) =>
        Expression.Throw(value, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Catch(System.Linq.Expressions.ParameterExpression,System.Linq.Expressions.Expression)" />
    public static CatchBlock ECatch(this ParameterExpression variable, Expression body) =>
        Expression.Catch(variable, body);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Catch(System.Linq.Expressions.ParameterExpression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static CatchBlock ECatch(this ParameterExpression variable, Expression body, Expression filter) =>
        Expression.Catch(variable, body, filter);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression},System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static BlockExpression EBlock(this Type type, IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions) =>
        Expression.Block(type, variables, expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression},System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static BlockExpression EBlock<T>(this IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions) =>
        Expression.Block(typeof(T), variables, expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static BlockExpression EBlock(this IEnumerable<Expression> expressions) =>
        Expression.Block(expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Type,System.Linq.Expressions.Expression[])" />
    public static BlockExpression EBlock(this Type type, params Expression[] expressions) =>
        Expression.Block(type, expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Type,System.Linq.Expressions.Expression[])" />
    public static BlockExpression EBlock<T>(this Expression[] expressions) =>
        Expression.Block(typeof(T), expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static BlockExpression EBlock(this Type type, IEnumerable<Expression> expressions) =>
        Expression.Block(type, expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static BlockExpression EBlock<T>(this IEnumerable<Expression> expressions) =>
        Expression.Block(typeof(T), expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression},System.Linq.Expressions.Expression[])" />
    public static BlockExpression EBlock(this IEnumerable<ParameterExpression> variables, params Expression[] expressions) =>
        Expression.Block(variables, expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression},System.Linq.Expressions.Expression[])" />
    public static BlockExpression EBlock(this Type type, IEnumerable<ParameterExpression> variables, params Expression[] expressions) =>
        Expression.Block(type, variables, expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression},System.Linq.Expressions.Expression[])" />
    public static BlockExpression EBlock<T>(this IEnumerable<ParameterExpression> variables, params Expression[] expressions) =>
        Expression.Block(typeof(T), variables, expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Block(System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression},System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static BlockExpression EBlock(this IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions) =>
        Expression.Block(variables, expressions);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Break(System.Linq.Expressions.LabelTarget)" />
    public static GotoExpression EBreak(this LabelTarget target) =>
        Expression.Break(target);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Break(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression)" />
    public static GotoExpression EBreak(this LabelTarget target, Expression value) =>
        Expression.Break(target, value);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Break(System.Linq.Expressions.LabelTarget,System.Type)" />
    public static GotoExpression EBreak(this LabelTarget target, Type type) =>
        Expression.Break(target, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Break(System.Linq.Expressions.LabelTarget,System.Type)" />
    public static GotoExpression EBreak<T>(this LabelTarget target) =>
        Expression.Break(target, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Break(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression,System.Type)" />
    public static GotoExpression EBreak(this LabelTarget target, Expression value, Type type) =>
        Expression.Break(target, value, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Break(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression,System.Type)" />
    public static GotoExpression EBreak<T>(this LabelTarget target, Expression value) =>
        Expression.Break(target, value, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Type,System.String,System.Type[],System.Linq.Expressions.Expression[])" />
    public static MethodCallExpression ECall(this Type type, string methodName, Type[] typeArguments, params Expression[] arguments) =>
        Expression.Call(type, methodName, typeArguments, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Call(System.Type,System.String,System.Type[],System.Linq.Expressions.Expression[])" />
    public static MethodCallExpression ECall<T>(this string methodName, Type[] typeArguments, params Expression[] arguments) =>
        Expression.Call(typeof(T), methodName, typeArguments, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Catch(System.Type,System.Linq.Expressions.Expression)" />
    public static CatchBlock ECatch(this Type type, Expression body) =>
        Expression.Catch(type, body);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Catch(System.Type,System.Linq.Expressions.Expression)" />
    public static CatchBlock ECatch<T>(this Expression body) =>
        Expression.Catch(typeof(T), body);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Catch(System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static CatchBlock ECatch(this Type type, Expression body, Expression filter) =>
        Expression.Catch(type, body, filter);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Catch(System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static CatchBlock ECatch<T>(this Expression body, Expression filter) =>
        Expression.Catch(typeof(T), body, filter);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Constant(System.Object)" />
    public static ConstantExpression EConst(this object value) =>
        Expression.Constant(value);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Constant(System.Object,System.Type)" />
    public static ConstantExpression EConst(this object value, Type type) =>
        Expression.Constant(value, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Constant(System.Object,System.Type)" />
    public static ConstantExpression EConst<T>(this object value) =>
        Expression.Constant(value, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Continue(System.Linq.Expressions.LabelTarget)" />
    public static GotoExpression EContinue(this LabelTarget target) =>
        Expression.Continue(target);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Continue(System.Linq.Expressions.LabelTarget,System.Type)" />
    public static GotoExpression EContinue(this LabelTarget target, Type type) =>
        Expression.Continue(target, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Continue(System.Linq.Expressions.LabelTarget,System.Type)" />
    public static GotoExpression EContinue<T>(this LabelTarget target) =>
        Expression.Continue(target, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Default(System.Type)" />
    public static DefaultExpression EDefault(this Type type) =>
        Expression.Default(type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Goto(System.Linq.Expressions.LabelTarget)" />
    public static GotoExpression EGoto(this LabelTarget target) =>
        Expression.Goto(target);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Goto(System.Linq.Expressions.LabelTarget,System.Type)" />
    public static GotoExpression EGoto(this LabelTarget target, Type type) =>
        Expression.Goto(target, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Goto(System.Linq.Expressions.LabelTarget,System.Type)" />
    public static GotoExpression EGoto<T>(this LabelTarget target) =>
        Expression.Goto(target, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Goto(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression)" />
    public static GotoExpression EGoto(this LabelTarget target, Expression value) =>
        Expression.Goto(target, value);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Goto(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression,System.Type)" />
    public static GotoExpression EGoto(this LabelTarget target, Expression value, Type type) =>
        Expression.Goto(target, value, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Goto(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression,System.Type)" />
    public static GotoExpression EGoto<T>(this LabelTarget target, Expression value) =>
        Expression.Goto(target, value, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Label(System.Linq.Expressions.LabelTarget)" />
    public static LabelExpression ELabel(this LabelTarget target) =>
        Expression.Label(target);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Label(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression)" />
    public static LabelExpression ELabel(this LabelTarget target, Expression defaultValue) =>
        Expression.Label(target, defaultValue);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Label(System.String)" />
    public static LabelTarget ELabel(this string name) =>
        Expression.Label(name);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Label(System.Type)" />
    public static LabelTarget ELabel(this Type type) =>
        Expression.Label(type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Label(System.Type,System.String)" />
    public static LabelTarget ELabel(this Type type, string name) =>
        Expression.Label(type, name);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Label(System.Type,System.String)" />
    public static LabelTarget ELabel<T>(this string name) =>
        Expression.Label(typeof(T), name);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.ParameterExpression[])" />
    public static LambdaExpression ELambda(this Type delegateType, Expression body, params ParameterExpression[] parameters) =>
        Expression.Lambda(delegateType, body, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Type,System.Linq.Expressions.Expression,System.Boolean,System.Linq.Expressions.ParameterExpression[])" />
    public static LambdaExpression ELambda(this Type delegateType, Expression body, Boolean tailCall, params ParameterExpression[] parameters) =>
        Expression.Lambda(delegateType, body, tailCall, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Type,System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static LambdaExpression ELambda(this Type delegateType, Expression body, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda(delegateType, body, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Type,System.Linq.Expressions.Expression,System.Boolean,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static LambdaExpression ELambda(this Type delegateType, Expression body, Boolean tailCall, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda(delegateType, body, tailCall, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Type,System.Linq.Expressions.Expression,System.String,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static LambdaExpression ELambda(this Type delegateType, Expression body, string name, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda(delegateType, body, name, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Lambda(System.Type,System.Linq.Expressions.Expression,System.String,System.Boolean,System.Collections.Generic.IEnumerable{System.Linq.Expressions.ParameterExpression})" />
    public static LambdaExpression ELambda(this Type delegateType, Expression body, string name, Boolean tailCall, IEnumerable<ParameterExpression> parameters) =>
        Expression.Lambda(delegateType, body, name, tailCall, parameters);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeBinary(System.Linq.Expressions.ExpressionType,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static BinaryExpression EMakeBinary(this ExpressionType binaryType, Expression left, Expression right) =>
        Expression.MakeBinary(binaryType, left, right);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeBinary(System.Linq.Expressions.ExpressionType,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Boolean,System.Reflection.MethodInfo)" />
    public static BinaryExpression EMakeBinary(this ExpressionType binaryType, Expression left, Expression right, Boolean liftToNull, MethodInfo method) =>
        Expression.MakeBinary(binaryType, left, right, liftToNull, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeBinary(System.Linq.Expressions.ExpressionType,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Boolean,System.Reflection.MethodInfo,System.Linq.Expressions.LambdaExpression)" />
    public static BinaryExpression EMakeBinary(this ExpressionType binaryType, Expression left, Expression right, Boolean liftToNull, MethodInfo method, LambdaExpression conversion) =>
        Expression.MakeBinary(binaryType, left, right, liftToNull, method, conversion);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeCatchBlock(System.Type,System.Linq.Expressions.ParameterExpression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static CatchBlock EMakeCatchBlock(this Type type, ParameterExpression variable, Expression body, Expression filter) =>
        Expression.MakeCatchBlock(type, variable, body, filter);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeCatchBlock(System.Type,System.Linq.Expressions.ParameterExpression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static CatchBlock EMakeCatchBlock<T>(this ParameterExpression variable, Expression body, Expression filter) =>
        Expression.MakeCatchBlock(typeof(T), variable, body, filter);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression[])" />
    public static DynamicExpression EMakeDynamic(this Type delegateType, CallSiteBinder binder, params Expression[] arguments) =>
        Expression.MakeDynamic(delegateType, binder, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression[])" />
    public static DynamicExpression EMakeDynamic<T>(this CallSiteBinder binder, params Expression[] arguments) =>
        Expression.MakeDynamic(typeof(T), binder, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static DynamicExpression EMakeDynamic(this Type delegateType, CallSiteBinder binder, IEnumerable<Expression> arguments) =>
        Expression.MakeDynamic(delegateType, binder, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static DynamicExpression EMakeDynamic<T>(this CallSiteBinder binder, IEnumerable<Expression> arguments) =>
        Expression.MakeDynamic(typeof(T), binder, arguments);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression)" />
    public static DynamicExpression EMakeDynamic(this Type delegateType, CallSiteBinder binder, Expression arg0) =>
        Expression.MakeDynamic(delegateType, binder, arg0);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression)" />
    public static DynamicExpression EMakeDynamic<T>(this CallSiteBinder binder, Expression arg0) =>
        Expression.MakeDynamic(typeof(T), binder, arg0);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static DynamicExpression EMakeDynamic(this Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1) =>
        Expression.MakeDynamic(delegateType, binder, arg0, arg1);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static DynamicExpression EMakeDynamic<T>(this CallSiteBinder binder, Expression arg0, Expression arg1) =>
        Expression.MakeDynamic(typeof(T), binder, arg0, arg1);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static DynamicExpression EMakeDynamic(this Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2) =>
        Expression.MakeDynamic(delegateType, binder, arg0, arg1, arg2);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static DynamicExpression EMakeDynamic<T>(this CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2) =>
        Expression.MakeDynamic(typeof(T), binder, arg0, arg1, arg2);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static DynamicExpression EMakeDynamic(this Type delegateType, CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2, Expression arg3) =>
        Expression.MakeDynamic(delegateType, binder, arg0, arg1, arg2, arg3);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeDynamic(System.Type,System.Runtime.CompilerServices.CallSiteBinder,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression)" />
    public static DynamicExpression EMakeDynamic<T>(this CallSiteBinder binder, Expression arg0, Expression arg1, Expression arg2, Expression arg3) =>
        Expression.MakeDynamic(typeof(T), binder, arg0, arg1, arg2, arg3);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeGoto(System.Linq.Expressions.GotoExpressionKind,System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression,System.Type)" />
    public static GotoExpression EMakeGoto(this GotoExpressionKind kind, LabelTarget target, Expression value, Type type) =>
        Expression.MakeGoto(kind, target, value, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeGoto(System.Linq.Expressions.GotoExpressionKind,System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression,System.Type)" />
    public static GotoExpression EMakeGoto<T>(this GotoExpressionKind kind, LabelTarget target, Expression value) =>
        Expression.MakeGoto(kind, target, value, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeTry(System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.CatchBlock})" />
    public static TryExpression EMakeTry(this Type type, Expression body, Expression @finally, Expression fault, IEnumerable<CatchBlock> handlers) =>
        Expression.MakeTry(type, body, @finally, fault, handlers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeTry(System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Collections.Generic.IEnumerable{System.Linq.Expressions.CatchBlock})" />
    public static TryExpression EMakeTry<T>(this Expression body, Expression @finally, Expression fault, IEnumerable<CatchBlock> handlers) =>
        Expression.MakeTry(typeof(T), body, @finally, fault, handlers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeUnary(System.Linq.Expressions.ExpressionType,System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EMakeUnary(this ExpressionType unaryType, Expression operand, Type type) =>
        Expression.MakeUnary(unaryType, operand, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeUnary(System.Linq.Expressions.ExpressionType,System.Linq.Expressions.Expression,System.Type)" />
    public static UnaryExpression EMakeUnary<T>(this ExpressionType unaryType, Expression operand) =>
        Expression.MakeUnary(unaryType, operand, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeUnary(System.Linq.Expressions.ExpressionType,System.Linq.Expressions.Expression,System.Type,System.Reflection.MethodInfo)" />
    public static UnaryExpression EMakeUnary(this ExpressionType unaryType, Expression operand, Type type, MethodInfo method) =>
        Expression.MakeUnary(unaryType, operand, type, method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.MakeUnary(System.Linq.Expressions.ExpressionType,System.Linq.Expressions.Expression,System.Type,System.Reflection.MethodInfo)" />
    public static UnaryExpression EMakeUnary<T>(this ExpressionType unaryType, Expression operand, MethodInfo method) =>
        Expression.MakeUnary(unaryType, operand, typeof(T), method);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.New(System.Type)" />
    public static NewExpression ENew(this Type type) =>
        Expression.New(type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NewArrayBounds(System.Type,System.Linq.Expressions.Expression[])" />
    public static NewArrayExpression ENewArrayBounds(this Type type, params Expression[] bounds) =>
        Expression.NewArrayBounds(type, bounds);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NewArrayBounds(System.Type,System.Linq.Expressions.Expression[])" />
    public static NewArrayExpression ENewArrayBounds<T>(this Expression[] bounds) =>
        Expression.NewArrayBounds(typeof(T), bounds);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NewArrayBounds(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static NewArrayExpression ENewArrayBounds(this Type type, IEnumerable<Expression> bounds) =>
        Expression.NewArrayBounds(type, bounds);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NewArrayBounds(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static NewArrayExpression ENewArrayBounds<T>(this IEnumerable<Expression> bounds) =>
        Expression.NewArrayBounds(typeof(T), bounds);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NewArrayInit(System.Type,System.Linq.Expressions.Expression[])" />
    public static NewArrayExpression ENewArrayInit(this Type type, params Expression[] initializers) =>
        Expression.NewArrayInit(type, initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NewArrayInit(System.Type,System.Linq.Expressions.Expression[])" />
    public static NewArrayExpression ENewArrayInit<T>(this Expression[] initializers) =>
        Expression.NewArrayInit(typeof(T), initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NewArrayInit(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static NewArrayExpression ENewArrayInit(this Type type, IEnumerable<Expression> initializers) =>
        Expression.NewArrayInit(type, initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.NewArrayInit(System.Type,System.Collections.Generic.IEnumerable{System.Linq.Expressions.Expression})" />
    public static NewArrayExpression ENewArrayInit<T>(this IEnumerable<Expression> initializers) =>
        Expression.NewArrayInit(typeof(T), initializers);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Parameter(System.Type)" />
    public static ParameterExpression EParameter(this Type type) =>
        Expression.Parameter(type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Parameter(System.Type,System.String)" />
    public static ParameterExpression EParameter(this Type type, string name) =>
        Expression.Parameter(type, name);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Parameter(System.Type,System.String)" />
    public static ParameterExpression EParameter<T>(this string name) =>
        Expression.Parameter(typeof(T), name);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Rethrow(System.Type)" />
    public static UnaryExpression ERethrow(this Type type) =>
        Expression.Rethrow(type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Return(System.Linq.Expressions.LabelTarget)" />
    public static GotoExpression EReturn(this LabelTarget target) =>
        Expression.Return(target);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Return(System.Linq.Expressions.LabelTarget,System.Type)" />
    public static GotoExpression EReturn(this LabelTarget target, Type type) =>
        Expression.Return(target, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Return(System.Linq.Expressions.LabelTarget,System.Type)" />
    public static GotoExpression EReturn<T>(this LabelTarget target) =>
        Expression.Return(target, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Return(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression)" />
    public static GotoExpression EReturn(this LabelTarget target, Expression value) =>
        Expression.Return(target, value);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Return(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression,System.Type)" />
    public static GotoExpression EReturn(this LabelTarget target, Expression value, Type type) =>
        Expression.Return(target, value, type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Return(System.Linq.Expressions.LabelTarget,System.Linq.Expressions.Expression,System.Type)" />
    public static GotoExpression EReturn<T>(this LabelTarget target, Expression value) =>
        Expression.Return(target, value, typeof(T));

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Switch(System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.SwitchCase[])" />
    public static SwitchExpression ESwitch(this Type type, Expression switchValue, Expression defaultBody, MethodInfo comparison, params SwitchCase[] cases) =>
        Expression.Switch(type, switchValue, defaultBody, comparison, cases);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Switch(System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Linq.Expressions.SwitchCase[])" />
    public static SwitchExpression ESwitch<T>(this Expression switchValue, Expression defaultBody, MethodInfo comparison, params SwitchCase[] cases) =>
        Expression.Switch(typeof(T), switchValue, defaultBody, comparison, cases);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Switch(System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Collections.Generic.IEnumerable{System.Linq.Expressions.SwitchCase})" />
    public static SwitchExpression ESwitch(this Type type, Expression switchValue, Expression defaultBody, MethodInfo comparison, IEnumerable<SwitchCase> cases) =>
        Expression.Switch(type, switchValue, defaultBody, comparison, cases);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Switch(System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Reflection.MethodInfo,System.Collections.Generic.IEnumerable{System.Linq.Expressions.SwitchCase})" />
    public static SwitchExpression ESwitch<T>(this Expression switchValue, Expression defaultBody, MethodInfo comparison, IEnumerable<SwitchCase> cases) =>
        Expression.Switch(typeof(T), switchValue, defaultBody, comparison, cases);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Variable(System.Type)" />
    public static ParameterExpression EVariable(this Type type) =>
        Expression.Variable(type);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Variable(System.Type,System.String)" />
    public static ParameterExpression EVariable(this Type type, string name) =>
        Expression.Variable(type, name);

    /// <inheritdoc cref="M:System.Linq.Expressions.Expression.Variable(System.Type,System.String)" />
    public static ParameterExpression EVariable<T>(this string name) =>
        Expression.Variable(typeof(T), name);

}