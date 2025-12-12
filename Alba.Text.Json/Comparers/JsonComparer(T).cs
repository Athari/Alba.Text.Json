using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using Alba.Text.Json.Dynamic;
using Alba.Text.Json.Extensions;

namespace Alba.Text.Json;

/// <summary>Comparer of JSON objects.</summary>
/// <typeparam name="T">The type of objects to compare. The type can be <see cref="IJNode"/>, <see cref="JsonNode"/>, <see cref="JsonElement"/>, <see cref="JsonDocument"/>, or anything that can be serialized to <see cref="JsonNode"/>.</typeparam>
public sealed class JsonComparer<T>(JEquality equality, JNodeOptions options)
    : IEqualityComparer<T?>, IEqualityComparer
{
    /// <summary>Kind of equality comparison.</summary>
    public JEquality Equality { get; } = equality;

    /// <summary>Options to control the behavior.</summary>
    public JNodeOptions Options { get; } = options;

    /// <inheritdoc/>
    public bool Equals(T? x, T? y) => JsonNode.Equals(x, y, Equality, Options);

    /// <inheritdoc/>
    public int GetHashCode(T? obj) => JsonNode.GetHashCode(obj, Equality, Options);

    /// <inheritdoc/>
    bool IEqualityComparer.Equals(object? x, object? y) => Equals((T?)x, (T?)y);

    /// <inheritdoc/>
    int IEqualityComparer.GetHashCode(object obj) => GetHashCode((T)obj);
}