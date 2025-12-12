using System.Collections;
using System.Text.Json;
using Alba.Text.Json.Extensions;

namespace Alba.Text.Json;

/// <summary>Comparer of <see cref="JsonDocument"/>.</summary>
public sealed class JsonDocumentComparer(JEquality equality, JNodeOptions options)
    : IEqualityComparer<JsonDocument?>, IEqualityComparer
{
    /// <inheritdoc cref="JsonComparer.Reference"/>
    [field: MaybeNull]
    public static JsonDocumentComparer Reference => field ??= new(JEquality.Reference, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Shallow"/>
    [field: MaybeNull]
    public static JsonDocumentComparer Shallow => field ??= new(JEquality.Shallow, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Deep"/>
    [field: MaybeNull]
    public static JsonDocumentComparer Deep => field ??= new(JEquality.Deep, JNodeOptions.Default);

    /// <summary>Kind of equality comparison.</summary>
    public JEquality Equality { get; } = equality;

    /// <summary>Options to control the behavior.</summary>
    public JNodeOptions Options { get; } = options;

    /// <inheritdoc/>
    public bool Equals(JsonDocument? x, JsonDocument? y) => JsonElement.Equals(x, y, Equality, Options);

    /// <inheritdoc/>
    public int GetHashCode(JsonDocument? obj) => obj?.RootElement.GetHashCode(Equality, Options) ?? 0;

    /// <inheritdoc/>
    bool IEqualityComparer.Equals(object? x, object? y) => Equals((JsonDocument)x!, (JsonDocument)y!);

    /// <inheritdoc/>
    int IEqualityComparer.GetHashCode(object obj) => GetHashCode((JsonDocument)obj);
}