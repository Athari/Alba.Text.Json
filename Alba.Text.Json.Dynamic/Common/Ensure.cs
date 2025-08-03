using System.Runtime.CompilerServices;
using MethodAttribute = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Alba.Text.Json.Dynamic;

[InternalAPI]
internal static class Ensure
{
    private const MethodImplOptions Inline = MethodImplOptions.AggressiveInlining;

    [Method(Inline), ContractAnnotation("param:null => halt")]
    public static T NotNull<T>([NotNull] T? param,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (param is not null)
            return param;
        throw new ArgumentNullException(paramName, $"Argument {paramName} must not be null");
    }

    [Method(Inline), ContractAnnotation("param:null => halt")]
    public static T NotNull<T>([NotNull] T? param,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
        where T : struct
    {
        if (param is not null)
            return param.Value;
        throw new ArgumentNullException(paramName, $"Argument {paramName} must not be null");
    }

    [Method(Inline)]
    public static int GreaterThanOrEqualTo(int param, int value,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (param >= value)
            return param;
        throw new ArgumentOutOfRangeException(paramName, param, $"Argument {paramName} must be greater than our equal to ${value}.");
    }

    [Method(Inline)]
    public static int LessThanOrEqualTo(int param, int value,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (param <= value)
            return param;
        throw new ArgumentOutOfRangeException(paramName, param, $"Argument {paramName} must be less than our equal to ${value}.");
    }

    [Method(Inline)]
    public static T[] Count<T>(T[] param, int value,
        [InvokerParameterName, CallerArgumentExpression(nameof(param))] string? paramName = null)
    {
        if (param.Length == value)
            return param;
        throw new ArgumentOutOfRangeException(paramName, param, $"Argument {paramName} must contain ${value} elements.");
    }
}