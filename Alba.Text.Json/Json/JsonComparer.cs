using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using Alba.Text.Json.Dynamic.Extensions;

namespace Alba.Text.Json.Dynamic;

/// <summary>Container of predefined comparers of JSON objects using <see cref="JNodeOptions.Default"/> options.</summary>
public static class JsonComparer
{
    /// <summary>Gets a comparer instance that compares objects by <see cref="Equality.Reference"/> equality using <see cref="JNodeOptions.Default"/> options.</summary>
    [field: MaybeNull]
    public static JsonComparer<object> Reference => field ??= new(Equality.Reference, JNodeOptions.Default);

    /// <summary>Gets a comparer instance that compares objects by <see cref="Equality.Shallow"/> equality using <see cref="JNodeOptions.Default"/> options.</summary>
    [field: MaybeNull]
    public static JsonComparer<object> Shallow => field ??= new(Equality.Shallow, JNodeOptions.Default);

    /// <summary>Gets a comparer instance that compares objects by <see cref="Equality.Deep"/> equality using <see cref="JNodeOptions.Default"/> options.</summary>
    [field: MaybeNull]
    public static JsonComparer<object> Deep => field ??= new(Equality.Deep, JNodeOptions.Default);
}

/// <summary>Container of predefined comparers of <see cref="JsonNode"/> using <see cref="JNodeOptions.Default"/> options.</summary>
public static class JsonNodeComparer
{
    /// <inheritdoc cref="JsonComparer.Reference"/>
    [field: MaybeNull]
    public static JsonComparer<JsonNode> Reference => field ??= new(Equality.Reference, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Shallow"/>
    [field: MaybeNull]
    public static JsonComparer<JsonNode> Shallow => field ??= new(Equality.Shallow, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Deep"/>
    [field: MaybeNull]
    public static JsonComparer<JsonNode> Deep => field ??= new(Equality.Deep, JNodeOptions.Default);
}

/// <summary>Comparer of JSON objects.</summary>
/// <typeparam name="T">The type of objects to compare. The type can be <see cref="IJNode"/>, <see cref="JsonNode"/>, <see cref="JsonElement"/>, <see cref="JsonDocument"/>, or anything that can be serialized to <see cref="JsonNode"/>.</typeparam>
public sealed class JsonComparer<T>(Equality equality, JNodeOptions options)
    : IEqualityComparer<T?>, IEqualityComparer
{
    /// <summary>Kind of equality comparison.</summary>
    public Equality Equality { get; } = equality;

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

/// <summary>Comparer of <see cref="JsonElement"/>.</summary>
public sealed class JsonElementComparer(Equality equality, JNodeOptions options)
    : IEqualityComparer<JsonElement>, IEqualityComparer
{
    /// <inheritdoc cref="JsonComparer.Reference"/>
    [field: MaybeNull]
    public static JsonElementComparer Reference => field ??= new(Equality.Reference, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Shallow"/>
    [field: MaybeNull]
    public static JsonElementComparer Shallow => field ??= new(Equality.Shallow, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Deep"/>
    [field: MaybeNull]
    public static JsonElementComparer Deep => field ??= new(Equality.Deep, JNodeOptions.Default);

    /// <summary>Kind of equality comparison.</summary>
    public Equality Equality { get; } = equality;

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

/// <summary>Comparer of <see cref="JsonDocument"/>.</summary>
public sealed class JsonDocumentComparer(Equality equality, JNodeOptions options)
    : IEqualityComparer<JsonDocument?>, IEqualityComparer
{
    /// <inheritdoc cref="JsonComparer.Reference"/>
    [field: MaybeNull]
    public static JsonDocumentComparer Reference => field ??= new(Equality.Reference, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Shallow"/>
    [field: MaybeNull]
    public static JsonDocumentComparer Shallow => field ??= new(Equality.Shallow, JNodeOptions.Default);

    /// <inheritdoc cref="JsonComparer.Deep"/>
    [field: MaybeNull]
    public static JsonDocumentComparer Deep => field ??= new(Equality.Deep, JNodeOptions.Default);

    /// <summary>Kind of equality comparison.</summary>
    public Equality Equality { get; } = equality;

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