using System.Collections;
using System.Text.Json;
using Alba.Text.Json.Extensions;

namespace Alba.Text.Json;

/// <summary>Comparer of <see cref="JsonElement"/>.</summary>
public sealed class JsonElementComparer(JEquality equality, JNodeOptions options)
    : IEqualityComparer<JsonElement>, IEqualityComparer
{
    /// <inheritdoc cref="JsonComparer.Reference"/>
    [field: MaybeNull]
    public static JsonElementComparer Reference => field ??= new(JEquality.Reference, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Shallow"/>
    [field: MaybeNull]
    public static JsonElementComparer Shallow => field ??= new(JEquality.Shallow, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Deep"/>
    [field: MaybeNull]
    public static JsonElementComparer Deep => field ??= new(JEquality.Deep, JNodeOptions.Default);

    /// <summary>Kind of equality comparison.</summary>
    public JEquality Equality { get; } = equality;

    /// <summary>Options to control the behavior.</summary>
    public JNodeOptions Options { get; } = options;

    /// <inheritdoc/>
    public bool Equals(JsonElement x, JsonElement y) => JsonElement.Equals(x, y, Equality, Options);

    /// <inheritdoc/>
    public int GetHashCode(JsonElement obj) => obj.GetHashCode(Equality, Options);

    /// <inheritdoc/>
    bool IEqualityComparer.Equals(object? x, object? y) => Equals((JsonElement)x!, (JsonElement)y!);

    /// <inheritdoc/>
    int IEqualityComparer.GetHashCode(object obj) => GetHashCode((JsonElement)obj);
}