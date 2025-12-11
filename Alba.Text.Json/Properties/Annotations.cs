// ReSharper disable CheckNamespace

namespace JetBrains.Annotations;

[InternalAPI]
[MeansImplicitUse(ImplicitUseTargetFlags.Members)]
[AttributeUsage(AttributeTargets.All, Inherited = false)]
internal sealed class InternalAPIAttribute(string? comment = null) : Attribute
{
    public string? Comment { get; } = comment;
}